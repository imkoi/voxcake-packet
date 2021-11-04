using System;
using System.Collections.Generic;
using System.Linq;

namespace VoxCake.Packet
{
    internal class PacketQueue
    {
        private readonly Dictionary<Type, List<Packet>> _publicPackets;
        private readonly Dictionary<byte, Dictionary<Type, List<Packet>>> _privatePackets;

        public PacketQueue()
        {
            _publicPackets = new Dictionary<Type, List<Packet>>();
            _privatePackets = new Dictionary<byte, Dictionary<Type, List<Packet>>>();
        }

        internal void SetPacketsForClientToCollection(byte clientId, List<Packet> collection)
        {
            var packetCollections = _publicPackets.Values.ToArray();
            
            foreach (var packetCollectionList in packetCollections)
            {
                var packetCollection = packetCollectionList.ToArray();
                foreach (var packet in packetCollection)
                {
                    collection.Add(packet);
                }
            }
            
            if (_privatePackets.ContainsKey(clientId))
            {
                var clientPackets = _privatePackets[clientId];

                foreach (var packetPair in clientPackets)
                {
                    var packetCollection = packetPair.Value;

                    foreach (var packet in packetCollection)
                    {
                        collection.Add(packet);
                    }
                }
            }
        }

        internal void AddPacket<TPacket>(TPacket packet, byte packetId, byte senderId, long timestamp)
            where TPacket : Packet
        {
            var packetType = packet.GetType();
            
            packet.id = packetId;
            packet.senderId = senderId;

            if (!_publicPackets.ContainsKey(packetType))
            {
                _publicPackets.Add(packetType, new List<Packet>
                {
                    packet
                });
            }
            else
            {
                var packets = _publicPackets[packetType];

                if (packet is PacketMergeable<TPacket> mergeablePacket)
                {
                    MergePackets(mergeablePacket, packets);

                    packets.Clear();
                }

                packets.Add(packet);
            }
        }
        
        internal void AddPacketForClient<TPacket>(
            TPacket packet,
            byte clientId,
            byte packetId, 
            byte senderId,
            long timestamp)
            where TPacket : Packet
        {
            var packetType = packet.GetType();

            packet.id = packetId;
            packet.senderId = senderId;
            packet.timestamp = timestamp;

            Dictionary<Type, List<Packet>> privatePackets = null;
            List<Packet> packets = null;
            
            if (!_privatePackets.ContainsKey(clientId))
            {
                privatePackets = new Dictionary<Type, List<Packet>>();
                _privatePackets.Add(clientId, privatePackets);
            }
            else
            {
                privatePackets = _privatePackets[clientId];
            }
            
            if (!privatePackets.ContainsKey(packetType))
            {
                packets = new List<Packet>();
                privatePackets.Add(packetType, packets);
            }
            else
            {
                packets = privatePackets[packetType];
            }
            
            if (packet is PacketMergeable<TPacket> mergeablePacket)
            {
                MergePackets(mergeablePacket, packets);

                packets.Clear();
            }

            packets.Add(packet);
        }

        private void MergePackets<TPacket>(PacketMergeable<TPacket> mergablePacket, List<Packet> packets)
            where TPacket : Packet
        {
            foreach (var otherPacket in packets)
            {
                mergablePacket.Merge((TPacket)otherPacket);

                if (otherPacket.timestamp > mergablePacket.timestamp)
                {
                    mergablePacket.timestamp = otherPacket.timestamp;
                }
            }
        }

        internal void Clear()
        {
            _publicPackets.Clear();
            _privatePackets.Clear();
        }
    }
}