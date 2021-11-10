using System;

namespace VoxCake.Packet.Tests.Packets
{
    public class TestPacket__2Handler : PacketHandler<TestPacket_2>
    {
        public override void Execute(TestPacket_2 packet)
        {
            Console.WriteLine($"PACKET 2 IS EXECUTED! pussy[{packet.pussy}] cat[{packet.cat}]");
        }
    }
}