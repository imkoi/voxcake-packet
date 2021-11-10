using VoxCake.Serialization;

namespace VoxCake.Packet.Tests.Packets
{
    public struct TestPacket_1 : IPacket
    {
        public int age;
        public string name;
        public long time;
        public short dick;
			
        public void Serialize(ref SerializationStream stream)
        {
            stream.WriteInt32(age);
            stream.WriteString(name);
            stream.WriteInt64(time);
            stream.WriteInt16(dick);
        }

        public void Deserialize(ref SerializationStream stream)
        {
            age = stream.ReadInt32();
            name = stream.ReadString();
            time = stream.ReadInt64();
            dick = stream.ReadInt16();
        }

        public void SkipDeserialization(ref SerializationStream stream)
        {
            stream.SkipInt32();
            stream.SkipString();
            stream.SkipInt64();
            stream.SkipInt16();
        }
    }
}