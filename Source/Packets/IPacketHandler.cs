namespace VoxCake.Packet
{
    internal interface IPacketHandler
    {
        void Execute(Packet packet);
    }
}