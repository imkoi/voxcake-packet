namespace VoxCake.Packet
{
    public interface IPacketMergeable<in TPacket> : IPacket where TPacket : struct, IPacket
    {
        void Merge(TPacket otherPacket);
    }
}