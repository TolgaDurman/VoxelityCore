using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataInfo
    {
        public string root;
        public string objectName;
        public DataTypes type;

        public DataInfo(string root ,string name, DataTypes type)
        {
            this.objectName = name;
            this.type = type;
        }
    }
    public enum DataTypes
    {
        Integer,
        String,
        Float,
        Bool,
        Vector3,
    }
}
