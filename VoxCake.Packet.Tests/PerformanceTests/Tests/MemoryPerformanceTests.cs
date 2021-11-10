using System.Diagnostics;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace VoxCake.Packet.Tests
{
    [TestFixture]
    public class MemoryPerformanceTests
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        
        private readonly IPacketProtocol _protocol = new PacketProtocol();
        private readonly IPacketHandlerCollection _handlerCollection = new PacketHandlerCollection();

        private readonly PlayerPacketHandler _playerPacketHandler = new PlayerPacketHandler();
        private readonly DamagePacketHandler _damagePacketHandler = new DamagePacketHandler();
        private readonly DamagePacketHandler _buildPacketHandler = new DamagePacketHandler();
        
        private IPacketQueue _packetQueue;

        public MemoryPerformanceTests()
        {
            _packetQueue = new PacketQueue(_protocol, _handlerCollection);
            
            _protocol.BindPacket<PlayerPacket>();
            _protocol.BindPacket<DamagePacket>();
            _protocol.BindPacket<BuildPacket>();
            
            _handlerCollection.AddHandler(_playerPacketHandler);
            _handlerCollection.AddHandler(_damagePacketHandler);
            _handlerCollection.AddHandler(_buildPacketHandler);
        }

        [AssertTraffic(AllocatedSizeInBytes = 2995)]
        [Test]
        [TestCase(16)]
        [TestCase(24)]
        [TestCase(32)]
        [TestCase(64)]
        public void TheTest(int playerCount)
        {
            _stopwatch.Restart();
            
            for (var i = 0; i < playerCount; i++)
            {
                _packetQueue.PushPacket(new PlayerPacket
                {
                    timestamp = (ushort)_stopwatch.ElapsedTicks,
                    xPosition = 332,
                    yPosition = 32,
                    zPosition = 314,
                    xRotation = 1232,
                    yRotation = 132,
                    zRotation = 1314,
                    state = 132
                });
            
                _packetQueue.PushPacket(new DamagePacket
                {
                    timestamp = (ushort)_stopwatch.ElapsedTicks,
                    damagedPlayerId = 132441551,
                    damageValue = 100
                });
            
                _packetQueue.PushPacket(new BuildPacket
                {
                    timestamp = (ushort)_stopwatch.ElapsedTicks,
                    pattern = 23,
                    prefabIndex = 31,
                    team = 12,
                    startBuildIndex = 324141,
                    endBuildIndex = 54363
                });
            }
        }
    }
}