using System.IO;
using UnityEngine;

namespace Voxelity.Save
{
    public static class SavableInfo
    {
        public const string c_AssetMenuHead = "Voxelity/Savables/";
        public static string WithPersistentDataPath(this string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
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