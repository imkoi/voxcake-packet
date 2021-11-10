using System.Collections.Generic;
using VoxCake.Serialization;

namespace VoxCake.Packet
{
    public class PacketQueue : IPacketQueue
    {
        private readonly int _protocolHash;
        
        private readonly IPacketProtocol _protocol;
        private readonly IPacketHandlerCollection _handlerCollection;

        private readonly Queue<IPacket> _sendPacketsQueue;
        private readonly Queue<IPacket> _receivedPacketsQueue;

        private SerializationStream _sendStream;

        public PacketQueue(IPacketProtocol protocol, IPacketHandlerCollection handlerCollection)
        {
            _protocolHash = protocol.GetHashCode();
            
            _protocol = protocol;
            _handlerCollection = handlerCollection;

            _sendPacketsQueue = new Queue<IPacket>();
            _receivedPacketsQueue = new Queue<IPacket>();
            
            _sendStream = SerializationStream.Create(1024 * 32);
            _sendStream.WriteInt32(_protocolHash);
        }

        public void PushPacket<TPacket>(TPacket packet) where TPacket : struct, IPacket
        {
            var packetBinding = _protocol.GetBinding<TPacket>();
            var packetHandlers = _handlerCollection.GetHandlers<TPacket>();
            
            if (packetHandlers != null)
            {
                var handlersCount = packetHandlers.Count;
                
                for (var i = 0; i < handlersCount; i++)
                {
                    var strictPacketHandler = (PacketHandler<TPacket>) packetHandlers[i];
                    
                    strictPacketHandler.Execute(packet);
                }
            }

            _sendStream.WriteByte(packetBinding.PacketId);
            packet.Serialize(ref _sendStream);
        }

        public byte[] GetSendBuffer()
        {
            var sendBuffer = _sendStream.ToArray();
            
            _sendStream.Clear();
            _sendStream.WriteInt32(_protocolHash);
            
            return sendBuffer;
        }

        public void HandleReceivedBuffer(byte[] receivedBuffer)
        {
            var stream = SerializationStream.Create(receivedBuffer);

            var receivedProtocolHash = stream.ReadInt32();

            if (_protocolHash == receivedProtocolHash)
            {
                while (!stream.IsEmpty)
                {
                    var packetId = stream.ReadByte();
                    var packetBinding = _protocol.GetBindingById(packetId);
                    var packet = packetBinding.Instance;
                    var packetType = packetBinding.Type;

                    var packetHandlers = _handlerCollection.GetHandlers(packetType);

                    if (packetHandlers != null && packetHandlers.Count > 0)
                    {
                        packet.Deserialize(ref stream);
                    
                        foreach (var packetHandler in packetHandlers)
                        {
                            packetHandler.Execute(packet);
                        }
                    }
                    else
                    {
                        packet.SkipDeserialization(ref stream);
                    }
                }
            }
        }
    }
}