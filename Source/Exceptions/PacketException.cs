using System;

namespace VoxCake.Packet
{
    internal class PacketException : Exception
    {
        internal PacketException(string message) : base(message)
        {
            
        }

        internal PacketException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}