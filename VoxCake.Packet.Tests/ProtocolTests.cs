using NUnit.Framework;

namespace VoxCake.Packet.Tests
{
	[TestFixture]
	public class ProtocolTests
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

		public class TestPacket_1 : Packet
		{
			public int age;
			public string name;
			public long time;
			public short dick;
		}
		
		public class TestPacket_1_Wrong : Packet
		{
			public byte age;
			public string name;
			public long time;
			public short dick;
		}
		
		public class TestPacket_2 : Packet
		{
			public string pussy;
			public string cat;
		}
	}
}