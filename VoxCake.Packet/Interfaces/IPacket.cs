using VoxCake.Serialization;

namespace VoxCake.Packet
{
    public interface IPacket
    {
        void Serialize(ref SerializationStream stream);
        void Deserialize(ref SerializationStream stream);
        void SkipDeserialization(ref SerializationStream stream);
    }
}