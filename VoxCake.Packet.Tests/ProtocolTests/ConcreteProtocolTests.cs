using System.Diagnostics;
using NUnit.Framework;
using VoxCake.Packet.Tests.Packets;

namespace VoxCake.Packet.Tests
{
	[TestFixture]
	public class ConcreteProtocolTests
	{
		[Test]
		public void Protocol_Hash_EqualsTest()
		{
			var protocol1 = new PacketProtocol();
			
			protocol1.BindPacket<TestPacket_1>();
			protocol1.BindPacket<TestPacket_2>();

			var hashCode1 = protocol1.GetHashCode();

			var protocol2 = new PacketProtocol();
			
			protocol2.BindPacket<TestPacket_1>();
			protocol2.BindPacket<TestPacket_2>();
			
			var hashCode2 = protocol2.GetHashCode();

			Assert.True(hashCode1 == hashCode2);
		}
		
		[Test]
		public void Integration_Test()
		{
			var protocol = new PacketProtocol();
			var handlerCollection = new PacketHandlerCollection();
			var packetQueue = new PacketQueue(protocol, handlerCollection);
			var sw = Stopwatch.StartNew();

			var handler1 = new TestPacket_1Handler();
			var handler2 = new TestPacket__2Handler();
			
			protocol.BindPacket<TestPacket_1>();
			protocol.BindPacket<TestPacket_2>();
			
			handlerCollection.AddHandler(handler1);
			handlerCollection.AddHandler(handler2);

			packetQueue.PushPacket(new TestPacket_1
			{
				age = 20,
				dick = 4,
				name = "Koi",
				time = sw.ElapsedTicks
			});
			
			packetQueue.PushPacket(new TestPacket_2
			{
				pussy = "ХАЙ)",
				cat = "МАЙ BABY zy@bl$"
			});
			
			packetQueue.PushPacket(new TestPacket_1
			{
				age = 54,
				dick = -13,
				name = "IRYNA",
				time = sw.ElapsedTicks
			});

			var sendBuffer = packetQueue.GetSendBuffer();
			
			packetQueue.HandleReceivedBuffer(sendBuffer);
			
			packetQueue.PushPacket(new TestPacket_1
			{
				age = 300,
				dick = 4,
				name = "КАЛЬЯННЫЙ МАСТИР",
				time = sw.ElapsedTicks
			});
			
			packetQueue.PushPacket(new TestPacket_2
			{
				pussy = "БИБАБОЙ",
				cat = "МАЙ BABY zy@bl$"
			});
			
			packetQueue.PushPacket(new TestPacket_1
			{
				age = 100,
				dick = -13,
				name = "ЧИНЧИЛА ЕНОТАВАЯ",
				time = sw.ElapsedTicks
			});
			
			sendBuffer = packetQueue.GetSendBuffer();
			
			packetQueue.HandleReceivedBuffer(sendBuffer);
		}
	}
}