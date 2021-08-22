using System.Collections.Generic;
using VoxCake.Serialization;

namespace VoxCake.Packet
{
    internal class PlayerMovementExample
    {
        private readonly IPacketSerializationController _packetSerializationController;
        private readonly IPacketHandlerController _packetHandlerController;

        public PlayerMovementExample()
        {
            var protocol = new PacketProtocol();
            var serializationController = new SerializationController();
            var floatSerializer = serializationController.GetTypeSerializer<float>();
            
            protocol.BindPacket<PlayerMovePacket>();
            serializationController.AddSerializer(new Vector3Serializer(floatSerializer));

            _packetSerializationController = new PacketSerializationController(protocol, serializationController);
            _packetHandlerController = new PacketHandlerController();
        }
        
        public void Initialize()
        {
            var player = new Player();
            var playerMoveHandler = new PlayerPacketHandler(player);
            
            _packetHandlerController.AddHandler(playerMoveHandler);
        }
        
        private void OnBufferReceived(byte[] buffer)
        {
            var packets = _packetSerializationController.Deserialize(buffer);

            foreach (var packet in packets)
            {
                _packetHandlerController.ExecutePacket(packet);
            }
        }
    }

    internal struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }

    internal class Vector3Serializer : Serializer<Vector3>
    {
        private readonly Serializer<float> _floatSerializer;

        public Vector3Serializer(Serializer<float> floatSerializer)
        {
            _floatSerializer = floatSerializer;
        }
        
        public override void Serialize(Vector3 variable, List<byte> buffer)
        {
            _floatSerializer.Serialize(variable.x, buffer);
            _floatSerializer.Serialize(variable.y, buffer);
            _floatSerializer.Serialize(variable.z, buffer);
        }

        public override Vector3 Deserialize(byte[] buffer, ref int index)
        {
            var x = _floatSerializer.Deserialize(buffer, ref index);
            var y = _floatSerializer.Deserialize(buffer, ref index);
            var z = _floatSerializer.Deserialize(buffer, ref index);

            return new Vector3
            {
                x = x,
                y = y,
                z = z
            };
        }
    }

    internal class Player
    {
        private Vector3 _position;
        
        public void SetPosition(Vector3 position)
        {
            _position = position;
        }
    }

    internal class PlayerMovePacket : Packet
    {
        public Vector3 position;
    }

    internal class PlayerPacketHandler : PacketHandler<PlayerMovePacket>
    {
        private readonly Player _player;

        public PlayerPacketHandler(Player player)
        {
            _player = player;
        }
        
        protected override void Execute(PlayerMovePacket packet)
        {
            _player.SetPosition(packet.position);
        }
    }
}