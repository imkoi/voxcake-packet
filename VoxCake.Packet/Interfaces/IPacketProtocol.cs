using System;

namespace VoxCake.Packet
{
    public interface IPacketProtocol
    {
        void BindPacket<TPacket>() where TPacket : struct, IPacket;
        IPacketBinding GetBinding<TPacket>() where TPacket : struct, IPacket;
        IPacketBinding GetBinding(Type packetType);
        IPacketBinding GetBindingById(int id);
    }
}