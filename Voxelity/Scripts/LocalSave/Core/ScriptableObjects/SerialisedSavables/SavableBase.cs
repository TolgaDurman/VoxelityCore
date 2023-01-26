using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public abstract class SavableBase<T> : ScriptableObject
    {
        public abstract SaveData<T> Data{get;set;}
        public T GetValue
        {
            get => Data.Value;
        }
    }
}
