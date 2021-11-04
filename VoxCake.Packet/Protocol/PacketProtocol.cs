using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VoxCake.Packet
{
    public class PacketProtocol : IPacketProtocol
    {
        private const BindingFlags FieldBindingFlags = BindingFlags.FlattenHierarchy 
                                                            | BindingFlags.Public 
                                                            | BindingFlags.Instance 
                                                            | BindingFlags.InvokeMethod;
        
        private readonly Dictionary<Type, IPacketBinding> _typeBindingPair;
        private readonly Dictionary<byte, IPacketBinding> _idBindingPair;

        public PacketProtocol()
        {
            _typeBindingPair = new Dictionary<Type, IPacketBinding>();
            _idBindingPair = new Dictionary<byte, IPacketBinding>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindPacket<TPacket>() where TPacket : Packet
        {
            var packetType = typeof(TPacket);

            if (!_typeBindingPair.ContainsKey(packetType))
            {
                var packetId = (byte)_typeBindingPair.Count;
                var packetFields = packetType.GetFields(FieldBindingFlags);
                var packetBinding = new PacketBinding(packetId, packetType, packetFields);
                
                _typeBindingPair.Add(packetType, packetBinding);
                _idBindingPair.Add(packetId, packetBinding);
            }
            else
            {
                throw new PacketException($"Packet \"{packetType.Name}\" already contains in protocol!");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBinding<TPacket>() where TPacket : Packet
        {
            var packetType = typeof(TPacket);

            return GetBinding(packetType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBinding(Type packetType)
        {
            if (!_typeBindingPair.ContainsKey(packetType))
            {
                throw new PacketException($"Packet binding for packet \"{packetType.Name}\" does not " +
                                          "exist in protocol!");
            }
            
            return _typeBindingPair[packetType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBindingById(byte id)
        {
            if (!_idBindingPair.ContainsKey(id))
            {
                throw new PacketException($"Packet binding for id \"{id}\" does not !" +
                                          "exist in protocol!");
            }
            
            return _idBindingPair[id];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            var hash = 0;
            var packetFieldIndex = 1;

            foreach (var packetPair in _typeBindingPair)
            {
                var packetType = packetPair.Key;
                var packetBinding = packetPair.Value;
                
                var packetId = packetPair.Value.Id;

                foreach (var packetField in packetBinding.Fields)
                {
                    hash += packetId * (Marshal.SizeOf(packetField.FieldType) / packetFieldIndex);
                    packetFieldIndex++;
                }
                
                hash += packetId * (Marshal.SizeOf(packetType) / packetFieldIndex);
            }
            
            return hash;
        }
    }
}