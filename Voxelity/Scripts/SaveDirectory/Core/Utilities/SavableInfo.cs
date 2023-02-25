using System.IO;
using UnityEngine;

namespace Voxelity.Save
{
    public static class SavableInfo
    {
        public const string c_AssetMenuHead = "Voxelity/Savables/";
        public static string WithPersistentSaveDataPath(this string fileName)
        {
            return Path.Combine(SavePath+"/", fileName);
        }
        public static string SavePath = Application.persistentDataPath+"/Saves";
        [System.Serializable]
        public enum ValueTypes
        {
            Integer,
            String,
            Float,
            Bool,
            Vector3,
        }
    }
}