using System;
using VoxCake.Packet.Tests.Packets;

namespace VoxCake.Packet.Tests
{
    public class TestPacket_1Handler : PacketHandler<TestPacket_1>
    {
        public override void Execute(TestPacket_1 packet)
        {
            Console.WriteLine($"PACKET 1 IS EXECUTED! age[{packet.age}] dick[{packet.dick}] name[{packet.name}] time[{packet.time}]");
        }
    }
}