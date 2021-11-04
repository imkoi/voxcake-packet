using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace VoxCake.Packet
{
    public class PacketBinding : IPacketBinding
    {
        public byte Id { get; }
        public Type Type { get; }
        public FieldInfo[] Fields { get; }
        public Packet Instance => (Packet)FormatterServices.GetSafeUninitializedObject(Type);

        public PacketBinding(byte packetId, Type packetType, FieldInfo[] packetFields)
        {
            Id = packetId;
            Type = packetType;
            Fields = packetFields;
        }
    }
}