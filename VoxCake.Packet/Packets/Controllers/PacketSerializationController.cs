using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VoxCake.Serialization;

namespace VoxCake.Packet
{
    public class PacketSerializationController : IPacketSerializationController
    {
        private readonly IPacketProtocol _packetProtocol;
        private readonly ISerializationController _serializationController;

        public PacketSerializationController(
            IPacketProtocol packetProtocol,
            ISerializationController serializationController)
        {
            _packetProtocol = packetProtocol;
            _serializationController = serializationController;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ref SerializationStream Serialize(Packet[] packets)
        {
            var stream = SerializationStream.Create(1024);
            var sortedPackets = packets.SortBySenderId(new Dictionary<int, List<Packet>>());
            
            stream.WriteInt(_packetProtocol.GetHashCode());

            foreach (var senderPacketPair in sortedPackets)
            {
                var clientId = senderPacketPair.Key;
                var clientPackets = senderPacketPair.Value;
                var packetsCount = (byte)clientPackets.Count;
                
                stream.AddRange(BitConverter.GetBytes(clientId));
                stream.Add(packetsCount);

                foreach (var packet in clientPackets)
                {
                    var packetId = packet.Id;
                    var packetTimestamp = packet.Timestamp;
                    var packetBinding = _packetProtocol.GetBindingById(packetId);
                    var packetFields = packetBinding.Fields;
                    
                    stream.Add(packetId);
                    stream.AddRange(BitConverter.GetBytes(packetTimestamp));

                    foreach (var field in packetFields)
                    {
                        var fieldType = field.FieldType;
                        var fieldValue = field.GetValue(packet);
                        
                        _serializationController.Serialize(fieldType, fieldValue, stream);
                    }
                }
            }

            return stream.ToArray();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Packet[] Deserialize(ref SerializationStream stream)
        {
            var packets = new List<Packet>(256);
            
            var protocolHashCode = stream.ReadInt();

            if (protocolHashCode == _packetProtocol.GetHashCode())
            {
                var bufferSize = buffer.Length;
                var index = 4;

                while (index < bufferSize)
                {
                    var senderId = BitConverter.ToInt32(buffer, index);
                    index += 4;
                    
                    var packetsCount = buffer[index++];

                    for (var packetIndex = 0; packetIndex < packetsCount; packetIndex++)
                    {
                        var packetId = buffer[index++];
                        var timestamp = BitConverter.ToInt64(buffer, index);
                        index += 8;
                        var binding = _packetProtocol.GetBindingById(packetId);
                        var packet = binding.Instance;
                        var packetType = binding.Type;
                        var packetFields = binding.Fields;

                        foreach (var field in packetFields)
                        {
                            var fieldType = field.FieldType;
                            var value = _serializationController.Deserialize(fieldType, buffer, ref index);
                            
                            field.SetValue(packet, value);
                        }

                        packet.id = packetId;
                        packet.senderId = senderId;

                        packets.Add(packet);
                    }
                }
            }

            return packets.ToArray();
        }
    }
}