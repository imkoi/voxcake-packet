using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public abstract class PacketMergeable<TPacket> : Packet where TPacket : Packet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Merge(TPacket otherPacket)
        {
            
        }
    }
}