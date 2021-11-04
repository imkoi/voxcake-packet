using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public static class PacketExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<ulong, List<Packet>> SortBySenderId(this Packet[] packets, Dictionary<ulong, List<Packet>> sortedPackets)
        {
            foreach (var packet in packets)
            {
                var senderId = packet.SenderId;
                List<Packet> clientPackets;

                if (!sortedPackets.ContainsKey(senderId))
                {
                    clientPackets = new List<Packet>();
                    sortedPackets.Add(senderId, clientPackets);
                }
                else
                {
                    clientPackets = sortedPackets[senderId];
                }
                
                clientPackets.Add(packet);
            }

            return sortedPackets;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsafeSetPacketId(this Packet packet, byte packetId)
        {
            packet.id = packetId;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsafeSetSenderId(this Packet packet, ulong senderId)
        {
            packet.senderId = senderId;
        }
    }
}