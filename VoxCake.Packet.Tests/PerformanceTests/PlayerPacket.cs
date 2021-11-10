using VoxCake.Serialization;

namespace VoxCake.Packet.Tests
{
    public struct PlayerPacket : IPacket
    {
        public ushort timestamp;
        
        public short xPosition;
        public short yPosition;
        public short zPosition;
        public short xRotation;
        public short yRotation;
        public short zRotation;
        public byte state;
        
        public void Serialize(ref SerializationStream stream)
        {
            stream.WriteUInt16(timestamp);
            
            stream.WriteInt16(xPosition);
            stream.WriteInt16(yPosition);
            stream.WriteInt16(zPosition);
            
            stream.WriteInt16(xRotation);
            stream.WriteInt16(yRotation);
            stream.WriteInt16(zRotation);
            
            stream.WriteByte(state);
        }

        public void Deserialize(ref SerializationStream stream)
        {
            timestamp = stream.ReadUInt16();
            
            xPosition = stream.ReadInt16();
            yPosition = stream.ReadInt16();
            zPosition = stream.ReadInt16();
            
            xRotation = stream.ReadInt16();
            yRotation = stream.ReadInt16();
            zRotation = stream.ReadInt16();
            
            state = stream.ReadByte();
        }

        public void SkipDeserialization(ref SerializationStream stream)
        {
            stream.SkipUInt16();
            
            stream.SkipInt16();
            stream.SkipInt16();
            stream.SkipInt16();
            
            stream.SkipInt16();
            stream.SkipInt16();
            stream.SkipInt16();
            
            stream.SkipByte();
        }
    }
}