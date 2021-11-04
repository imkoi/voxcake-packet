using VoxCake.Serialization;

namespace VoxCake.Packet
{
    public interface IPacketSerializationController
    {
        ref SerializationStream Serialize(Packet[] packets);
        Packet[] Deserialize(ref SerializationStream stream);
    }
}