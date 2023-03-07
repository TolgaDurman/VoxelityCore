using System.IO;
using UnityEngine;

namespace Voxelity.Save
{
    public static class SavableInfo
    {
        public const string c_AssetMenuHead = "Voxelity/Savables/";
        public static string WithPersistentDataPath(this string fileName)
        {
            return Path.Combine(SavePath+"/", fileName);
        }
        public static string WithProjectDataPath(this string fileName)
        {
            return Path.Combine(SaveToProjectPath+"/", fileName);
        }
        public static string SavePath = Application.persistentDataPath+"/Saves";
        public static string SaveToProjectPath = Application.dataPath.TrimEnd("Assets".ToCharArray())+"ProjectSettings/Voxelity";
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