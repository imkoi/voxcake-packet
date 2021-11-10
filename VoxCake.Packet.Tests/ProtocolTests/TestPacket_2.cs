using VoxCake.Serialization;

namespace VoxCake.Packet.Tests.Packets
{
    public struct TestPacket_2 : IPacket
    {
        public string pussy;
        public string cat;
			
        public void Serialize(ref SerializationStream stream)
        {
            stream.WriteString(pussy);
            stream.WriteString(cat);
        }

        public void Deserialize(ref SerializationStream stream)
        {
            pussy = stream.ReadString();
            cat = stream.ReadString();
        }

        public void SkipDeserialization(ref SerializationStream stream)
        {
            stream.SkipString();
            stream.SkipString();
        }
    }
}