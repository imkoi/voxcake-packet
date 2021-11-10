using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public abstract class PacketHandler<TPacket> : IPacketHandler where TPacket : struct, IPacket
    {
        public abstract void Execute(TPacket packet);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IPacketHandler.Execute(IPacket packet)
        {
            Execute((TPacket)packet);
        }
    }
}