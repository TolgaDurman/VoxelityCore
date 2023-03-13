using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Voxelity
{
    public class VoxelityException : Exception
    {
        public VoxelityException(string message) : base(message)
        {
        }

        public VoxelityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VoxelityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
