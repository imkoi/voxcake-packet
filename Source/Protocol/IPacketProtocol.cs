using System;

namespace VoxCake.Packet
{
    public interface IPacketProtocol
    {
        void BindPacket<TPacket>() where TPacket : Packet;
        IPacketBinding GetBinding<TPacket>() where TPacket : Packet;
        IPacketBinding GetBinding(Type packetType);
        IPacketBinding GetBindingById(byte id);
    }
}