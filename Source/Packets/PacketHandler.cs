using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public abstract class PacketHandler<TPacket> : IPacketHandler where TPacket : Packet
    {
        protected abstract void Execute(TPacket packet);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IPacketHandler.Execute(Packet packet)
        {
            Execute((TPacket)packet);
        }
    }
}