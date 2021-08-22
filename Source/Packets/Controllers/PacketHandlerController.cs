using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public class PacketHandlerController : IPacketHandlerController
    {
        private readonly Dictionary<Type, List<IPacketHandler>> _handlers;

        public PacketHandlerController()
        {
            _handlers = new Dictionary<Type, List<IPacketHandler>>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddHandler<TPacket>(PacketHandler<TPacket> packetHandler) 
            where TPacket : Packet
        {
            var packetType = typeof(TPacket);

            List<IPacketHandler> handlers;
                
            if (!_handlers.ContainsKey(packetType))
            {
                handlers = new List<IPacketHandler>();
                _handlers.Add(packetType, handlers);
            }
            else
            {
                handlers = _handlers[packetType];
            }

            handlers.Add(packetHandler);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveHandler<TPacket, THandler>()
            where TPacket : Packet
            where THandler : PacketHandler<TPacket>
        {
            foreach (var handlerPair in _handlers)
            {
                var handlers = handlerPair.Value;
                
                foreach (var handler in handlers)
                {
                    if (handler is THandler)
                    {
                        handlers.Remove(handler);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExecutePacket(Packet packet)
        {
            var packetType = packet.GetType();
                
            if (_handlers.ContainsKey(packetType))
            {
                var handlers = _handlers[packetType];

                if (handlers != null)
                {
                    foreach (var handler in handlers)
                    {
                        handler.Execute(packet);
                    }
                }
            }
        }
    }
}