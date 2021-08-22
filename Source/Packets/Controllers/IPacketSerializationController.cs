namespace VoxCake.Packet
{
    public interface IPacketSerializationController
    {
        byte[] Serialize(Packet[] packets);
        Packet[] Deserialize(byte[] buffer);
    }
}