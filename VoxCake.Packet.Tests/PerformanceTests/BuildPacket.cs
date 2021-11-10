using VoxCake.Serialization;

namespace VoxCake.Packet.Tests
{
    public struct BuildPacket : IPacket
    {
        public ushort timestamp;
        
        public byte team;
        public byte pattern;
        public byte prefabIndex;
        public int startBuildIndex;
        public int endBuildIndex;
        
        public void Serialize(ref SerializationStream stream)
        {
            stream.WriteUInt16(timestamp);
            
            stream.WriteByte(team);
            stream.WriteByte(pattern);
            stream.WriteByte(prefabIndex);
            
            stream.WriteInt32(startBuildIndex);
            stream.WriteInt32(endBuildIndex);
        }

        public void Deserialize(ref SerializationStream stream)
        {
            timestamp = stream.ReadUInt16();
            
            team = stream.ReadByte();
            pattern = stream.ReadByte();
            prefabIndex = stream.ReadByte();
            
            startBuildIndex = stream.ReadInt32();
            endBuildIndex = stream.ReadInt32();
        }

        public void SkipDeserialization(ref SerializationStream stream)
        {
            stream.SkipUInt16();
            
            stream.SkipByte();
            stream.SkipByte();
            stream.SkipByte();
            
            stream.SkipInt32();
            stream.SkipInt32();
        }
    }
}