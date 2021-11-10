namespace VoxCake.Packet
{
    public interface IPacketHandler
    {
        void Execute(IPacket packet);
    }
}