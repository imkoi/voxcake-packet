using VoxCake.Serialization;

namespace VoxCake.Packet.Tests
{
    public struct DamagePacket : IPacket
    {
        public ushort timestamp;
        
        public ulong damagedPlayerId;
        public byte damageValue;

        public void Serialize(ref SerializationStream stream)
        {
            stream.WriteUInt16(timestamp);
            
            stream.WriteUInt64(damagedPlayerId);
            stream.WriteByte(damageValue);
        }

        public void Deserialize(ref SerializationStream stream)
        {
            timestamp = stream.ReadUInt16();
            
            damagedPlayerId = stream.ReadUInt64();
            damageValue = stream.ReadByte();
        }

        public void SkipDeserialization(ref SerializationStream stream)
        {
            stream.SkipUInt16();
            
            stream.SkipUInt64();
            stream.SkipByte();
        }
    }
}