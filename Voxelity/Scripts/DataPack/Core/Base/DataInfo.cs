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

        public DataInfo(string root ,string name)
        {
            this.root = root;
            this.objectName = name;
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
