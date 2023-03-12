using System;

namespace Voxelity
{
    public class VoxelityException : Exception
    {
        public VoxelityException()
            : base()
        {
        }

        public VoxelityException(string message)
            : base(message)
        {
        }

        public VoxelityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}