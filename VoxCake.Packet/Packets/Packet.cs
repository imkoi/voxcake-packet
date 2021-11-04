namespace VoxCake.Packet
{
    public abstract class Packet
    {
        public byte Id => id;
        public ulong SenderId => senderId;

        internal byte id;
        internal ulong senderId;
    }
}