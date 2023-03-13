using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Voxelity.DataPacks.SaveDir
{
    [System.Serializable]
    public abstract class Savable<T> : Savables
    {
        public abstract T Value
        {
            get;
            set;
        }
        public override void Save()
        {
            directory.Write(this);
        }
        public override void Load()
        {
            directory.Load(this);
        }
    }
    public abstract class Savables : ScriptableObject
    {
        [JsonIgnore]public SaveDirectory directory;
        public abstract void Save();
        public void Commit() => directory.Commit();
        public abstract void Load();
    }
    public static class SavableExtension
    {
        public static T To<T>(this Savables self) where T : Savables
        {
            return (T)self;
        }
    }
}
