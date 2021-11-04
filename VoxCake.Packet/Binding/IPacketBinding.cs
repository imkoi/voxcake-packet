using System;
using System.Reflection;

namespace VoxCake.Packet
{
    public interface IPacketBinding
    {
        byte Id { get; }
        Type Type { get; }
        FieldInfo[] Fields { get; }
        Packet Instance { get; }
    }
}