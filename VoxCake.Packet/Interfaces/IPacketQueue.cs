namespace VoxCake.Packet
{
    public interface IPacketQueue
    {
        void PushPacket<TPacket>(TPacket packet) where TPacket : struct, IPacket;
        byte[] GetSendBuffer();
    }
}