using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public class PacketHandlerCollection : IPacketHandlerCollection
    {
        private readonly Dictionary<Type, List<IPacketHandler>> _handlers;

        public PacketHandlerCollection()
        {
            _handlers = new Dictionary<Type, List<IPacketHandler>>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddHandler<TPacket>(PacketHandler<TPacket> packetHandler) 
            where TPacket : struct, IPacket
        {
            var packetType = typeof(TPacket);

            List<IPacketHandler> handlers;
                
            if (_handlers.TryGetValue(packetType, out var value))
            {
                handlers = value;
            }
            else
            {
                handlers = new List<IPacketHandler>();
                _handlers.Add(packetType, handlers);
            }

            if (!handlers.Contains(packetHandler))
            {
                handlers.Add(packetHandler);
            }
            else
            {
                throw new PacketException($"Packet handler {packetHandler.GetType().Name} already exist in collection");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveHandler<TPacket, THandler>()
            where TPacket : struct, IPacket
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

        public List<IPacketHandler> GetHandlers<TPacket>() where TPacket : struct, IPacket
        {
            var packetType = typeof(TPacket);

            if (_handlers.TryGetValue(packetType, out var value))
            {
                return value;
            }

            return null;
        }
        
        public List<IPacketHandler> GetHandlers(Type packetType)
        {
            if (_handlers.TryGetValue(packetType, out var value))
            {
                return value;
            }

            return null;
        }
    }
}