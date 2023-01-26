using System;
using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public struct SaveData<T>
    {
        [SerializeField] private string _name;
        [SerializeField] private T _value;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public T Value
        {
            get => _value;
            set => _value = value;
        }
        public SaveData(string name, T value)
        {
            _name = name;
            _value = value;
        }
    }
}