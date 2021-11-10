using System;
using System.Reflection;

namespace VoxCake.Packet
{
    public class PacketBinding<TPacket> : IPacketBinding where TPacket : struct, IPacket
    {
        public byte PacketId { get; }
        public Type Type { get; }
        public FieldInfo[] Fields { get; }
        public IPacket Instance { get; }

        public PacketBinding(byte packetId, Type packetType, FieldInfo[] packetFields)
        {
            PacketId = packetId;
            Type = packetType;
            Fields = packetFields;
            Instance = new TPacket();
        }
    }
}