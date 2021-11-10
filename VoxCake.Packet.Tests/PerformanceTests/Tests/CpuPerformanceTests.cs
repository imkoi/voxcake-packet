using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace VoxCake.Packet.Tests
{
    [TestFixture]
    public class ConcretePerformanceTests
    {
        [Test]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(24)]
        [TestCase(32)]
        [TestCase(64)]
        public void TestManyPlayers(int playerCount)
        {
            var protocol = new PacketProtocol();
            var handlerCollection = new PacketHandlerCollection();
            var packetQueue = new PacketQueue(protocol, handlerCollection);

            var playerPacketHandler = new PlayerPacketHandler();
            var damagePacketHandler = new DamagePacketHandler();
            var buildPacketHandler = new DamagePacketHandler();
            
            protocol.BindPacket<PlayerPacket>();
            protocol.BindPacket<DamagePacket>();
            protocol.BindPacket<BuildPacket>();
            
            handlerCollection.AddHandler(playerPacketHandler);
            handlerCollection.AddHandler(damagePacketHandler);
            handlerCollection.AddHandler(buildPacketHandler);
            
            var sw = Stopwatch.StartNew();

            for (var i = 0; i < playerCount; i++)
            {
                packetQueue.PushPacket(new PlayerPacket
                {
                    timestamp = (ushort)sw.ElapsedTicks,
                    xPosition = 332,
                    yPosition = 32,
                    zPosition = 314,
                    xRotation = 1232,
                    yRotation = 132,
                    zRotation = 1314,
                    state = 132
                });
            
                packetQueue.PushPacket(new DamagePacket
                {
                    timestamp = (ushort)sw.ElapsedTicks,
                    damagedPlayerId = 132441551,
                    damageValue = 100
                });
            
                packetQueue.PushPacket(new BuildPacket
                {
                    timestamp = (ushort)sw.ElapsedTicks,
                    pattern = 23,
                    prefabIndex = 31,
                    team = 12,
                    startBuildIndex = 324141,
                    endBuildIndex = 54363
                });
            }

            var receivedPackets = packetQueue.GetSendBuffer();
            
            packetQueue.HandleReceivedBuffer(receivedPackets);
            
            sw.Stop();
            
            Console.WriteLine($"Flow for {playerCount} players = {(sw.ElapsedTicks / 10000f)}");
        }
    }
}