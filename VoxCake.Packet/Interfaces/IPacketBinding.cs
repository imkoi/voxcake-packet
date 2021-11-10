using System;
using System.Reflection;

namespace VoxCake.Packet
{
    public interface IPacketBinding
    {
        byte PacketId { get; }
        Type Type { get; }
        FieldInfo[] Fields { get; }
        IPacket Instance { get; }
    }
}