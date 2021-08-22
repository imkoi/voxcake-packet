namespace VoxCake.Packet
{
    public interface IPacketHandlerController
    {
        void AddHandler<TPacket>(PacketHandler<TPacket> packetHandler)
            where TPacket : Packet;
        void RemoveHandler<TPacket, THandler>()
            where TPacket : Packet
            where THandler : PacketHandler<TPacket>;

        void ExecutePacket(Packet packet);
    }
}