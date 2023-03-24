using System;
using System.Runtime.Serialization;
namespace Voxelity
{
    public class VoxelityException : Exception
    {
        public VoxelityException(string message) : base(message){}
        public VoxelityException(string message, Exception innerException) : base(message, innerException){}
        protected VoxelityException(SerializationInfo info, StreamingContext context) : base(info, context){}
    }
}
