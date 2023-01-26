using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Voxelity.Save
{
    public static class DataSaver
    {
        public static bool SavePDP(System.Object data, string fileName)
        {
            return Save(data, Application.persistentDataPath + fileName);
        }

        public static bool Save(System.Object data, string pathFileName)
        {
            try
            {
                using (FileStream file = File.Create(pathFileName))
                {
                    new BinaryFormatter().Serialize(file, data);
                }
                return true;
            }
            catch
            {
                if (File.Exists(pathFileName))
                {
                    File.Delete(pathFileName);
                }
                return false;
            }
        }

        public static System.Object LoadPDP(string fileName)
        {
            return Load(fileName.WithPersistentDataPath());
        }

        public static System.Object Load(string pathFileName)
        {
            if (!File.Exists(pathFileName))
            {
                return null;
            }
            try
            {
                using (FileStream file = File.Open(pathFileName, FileMode.Open))
                {
                    return new BinaryFormatter().Deserialize(file);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
