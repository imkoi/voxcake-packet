using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VoxCake.Packet
{
    public static class PacketExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<int, List<Packet>> SortBySenderId(this Packet[] packets, Dictionary<int, List<Packet>> sortedPackets)
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
        public static void UnsafeSetSenderId(this Packet packet, int senderId)
        {
            packet.senderId = senderId;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsafeSetTimestamp(this Packet packet, long timestamp)
        {
            packet.timestamp = timestamp;
        }
    }
}