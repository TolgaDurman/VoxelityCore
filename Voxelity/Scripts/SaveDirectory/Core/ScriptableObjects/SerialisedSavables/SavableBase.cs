using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public abstract class Savable<T> : Savables
    {
        public abstract T Value
        {
            get;
            set;
        }
    }
    [System.Serializable]
    public abstract class Savables : ScriptableObject
    {
        [SerializeField] public SaveDirectory directory;
        public void Save()
        {
            directory.Save();
        }
        public void Load()
        {
            directory.Load();
        }
    }
    public static class SavableExtension
    {
        public static T To<T>(this Savables self) where T : Savables
        {
            return (T)self;
        }
    }
}
