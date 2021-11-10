using System;
using System.Collections.Generic;

namespace VoxCake.Packet
{
    public interface IPacketHandlerCollection
    {
        void AddHandler<TPacket>(PacketHandler<TPacket> packetHandler)
            where TPacket : struct, IPacket;
        void RemoveHandler<TPacket, THandler>()
            where TPacket : struct, IPacket
            where THandler : PacketHandler<TPacket>;
        List<IPacketHandler> GetHandlers<TPacket>() where TPacket : struct, IPacket;
        List<IPacketHandler> GetHandlers(Type packetType);
    }
}