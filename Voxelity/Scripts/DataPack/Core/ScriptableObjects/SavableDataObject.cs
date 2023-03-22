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
        public T Value
        {
            get => Data.Value;
            set
            {
                Data.Value = value;
            }
        }
<<<<<<< Updated upstream
=======
        public override void SetToDefaultValue()
        {
            Data.LoadDefaultValue();
        }
>>>>>>> Stashed changes
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
        public abstract void SetToDefaultValue();
        public abstract void Save();
        public abstract void Load();
    } 
}
