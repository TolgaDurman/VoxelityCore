using System;
using UnityEngine;
using Voxelity.Extensions;

namespace Voxelity.Save
{
    [System.Serializable]
    public struct SaveData<T>
    {
        [SerializeField] private string m_Name;
        [SerializeField] private T m_Value;

        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
        public T Value
        {
            get => m_Value;
            set => m_Value = value;
        }
        public SaveData(string name, T value)
        {
            m_Name = name;
            m_Value = value;
        }
    }
}