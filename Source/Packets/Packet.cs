namespace VoxCake.Packet
{
    public abstract class Packet
    {
        public byte Id => id;
        public int SenderId => senderId;
        public long Timestamp => timestamp;

        internal byte id;
        internal int senderId;
        internal long timestamp;
    }
}