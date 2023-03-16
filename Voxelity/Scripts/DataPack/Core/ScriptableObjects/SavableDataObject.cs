using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelity.Saver;

namespace Voxelity.DataPacks
{
    [System.Serializable]
    public abstract class SavableDataObject<T> : SavableDataObjectBase
    {
        public Data<T> Data;
        public override void Save()
        {
            Data.Save();
        }
        public override void Load()
        {
            Data.Load();
        }
    }
    public abstract class SavableDataObjectBase : ScriptableObject
    {
        public abstract void Save();
        public abstract void Load();
    } 
}
