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
        private readonly IPacketBinding[] _bindings;

        public PacketProtocol()
        {
            _typeBindingPair = new Dictionary<Type, IPacketBinding>();
            _bindings = new IPacketBinding[256];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindPacket<TPacket>() where TPacket : struct, IPacket
        {
            var packetType = typeof(TPacket);

            if (!_typeBindingPair.ContainsKey(packetType))
            {
                var packetId = (byte)_typeBindingPair.Count;
                var packetFields = packetType.GetFields(FieldBindingFlags);
                var packetBinding = new PacketBinding<TPacket>(packetId, packetType, packetFields);
                
                _typeBindingPair.Add(packetType, packetBinding);
                _bindings[packetId] = packetBinding;
            }
            else
            {
                throw new PacketException($"Packet \"{packetType.Name}\" already contains in protocol!");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBinding<TPacket>() where TPacket : struct, IPacket
        {
            var packetType = typeof(TPacket);

            return GetBinding(packetType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBinding(Type packetType)
        {
            if (_typeBindingPair.TryGetValue(packetType, out var value))
            {
                return value;
            }
            
            throw new PacketException($"Packet binding for packet \"{packetType.Name}\" does not " +
                                      "exist in protocol!");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPacketBinding GetBindingById(int id)
        {
            var binding = _bindings[id];

            if (binding == null)
            {
                throw new PacketException($"Packet binding for id \"{id}\" does not !" +
                                          "exist in protocol!");
            }
            
            return binding;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            var hash = 0;

            try
            {
                var packetFieldIndex = 1;

                foreach (var packetPair in _typeBindingPair)
                {
                    var packetType = packetPair.Key;
                    var packetBinding = packetPair.Value;

                    var packetId = packetPair.Value.PacketId;

                    foreach (var packetField in packetBinding.Fields)
                    {
                        if (packetField.FieldType.IsClass)
                        {
                            hash += packetId * 8 / packetFieldIndex;
                        }
                        else
                        {
                            hash += packetId * (Marshal.SizeOf(packetField.FieldType) / packetFieldIndex);
                        }

                        packetFieldIndex++;
                    }

                    hash += packetId * (Marshal.SizeOf(packetType) / packetFieldIndex);
                }
            }
            catch
            {
                hash = -1;
            }
            
            return hash;
        }
    }
}