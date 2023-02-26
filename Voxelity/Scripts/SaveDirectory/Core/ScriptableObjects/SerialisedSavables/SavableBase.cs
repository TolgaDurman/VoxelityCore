using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public abstract class Savable<T> : Savables
    {
        internal SaveDirectory directory;
        public abstract T Value
        {
            get;
            set;
        }
        public override void Save()
        {
            directory.Save();
        }
        public override void Load()
        {
            directory.Load();
        }
    }
    [System.Serializable]
    public abstract class Savables : ScriptableObject
    {
        public abstract void Save();
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
