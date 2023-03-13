using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelity.Saver;

namespace Voxelity.DataPacks
{
    [System.Serializable]
    public abstract class SavableDataObject<T> : ScriptableObject
    {
        public string root = "Defaults";
        public string objectName;
        public T Value;
        public void Save()
        {
            var writer = VoxelitySaveWriter.Create(root);
            writer.Write(objectName, Value);
            writer.Commit();
        }
        public void Load()
        {
            var reader = VoxelitySaveReader.Create(root);
            Value = reader.Read<T>(objectName);
        }
    }
}
