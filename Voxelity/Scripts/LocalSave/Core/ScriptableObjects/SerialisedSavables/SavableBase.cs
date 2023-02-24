using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public abstract class SavableBase<T> : ScriptableObject
    {
        internal SaveDirectory directory;
        internal abstract SaveData<T> Data { get; set; }
        public T GetValue
        {
            get
            {
                Load();
                return Data.Value;
            }
        }
        public abstract T SetValue
        {
            set;
        }
        internal void Save()
        {
            directory.Save();
        }
        internal void Load()
        {
            directory.Load();
        }
    }
}
