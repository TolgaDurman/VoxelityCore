using UnityEngine;
using System.IO;
using Voxelity.Extensions.Utility;

namespace Voxelity.Save
{
    public static class JsonSaver
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            if (!FileUtility.Exists(SavableInfo.SavePath))
                FileUtility.CreateFolder(SavableInfo.SavePath);
#if UNITY_EDITOR
            if (!FileUtility.Exists(SavableInfo.SaveToProjectPath))
                FileUtility.CreateFolder(SavableInfo.SaveToProjectPath);
#endif
        }
        private const string c_Key = "JsonSaversaves";
        private static string GetPassword()
        {
            if (!PlayerPrefs.HasKey(c_Key))
            {
                PlayerPrefs.SetString(c_Key, CryptUtility.CreatePassword(16));
            }
            return PlayerPrefs.GetString(c_Key);
        }
        public static void Save<T>(string fileName, T data)
        {
            string json = "";
            json = JsonUtility.ToJson(data);
            File.WriteAllText(fileName.WithPersistentDataPath(), json);
        }
        public static void SaveRaw(string fileName, string data, bool toProject = false)
        {
            if (!toProject)
                File.WriteAllText(fileName.WithPersistentDataPath(), data);
            else
                File.WriteAllText(fileName.WithProjectDataPath(), data);
        }
        public static void SaveCrypted<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data);

            string encrypted;

            CryptUtility.EncryptAESWithECB(json, out encrypted, GetPassword());

            FileUtility.Save(fileName, bw =>
            {
                bw.Write(encrypted);
            });
        }
        public static void SaveCrypted<T>(string fileName, T data, string key)
        {
            string json = JsonUtility.ToJson(data);

            string encrypted;

            CryptUtility.EncryptAESWithECB(json, out encrypted, key);

            FileUtility.Save(fileName, bw =>
            {
                bw.Write(encrypted);
            });
        }

        public static T LoadCrypted<T>(string fileName)
        {
            string json = File.ReadAllText(fileName.WithPersistentDataPath());

            string decrypted;

            CryptUtility.DecryptAESWithECB(json, out decrypted, GetPassword());

            return JsonUtility.FromJson<T>(decrypted);
        }
        public static T LoadCrypted<T>(string fileName, string key)
        {
            string json = File.ReadAllText(fileName.WithPersistentDataPath());

            string decrypted;

            CryptUtility.DecryptAESWithECB(json, out decrypted, key);

            return JsonUtility.FromJson<T>(decrypted);
        }
        public static bool Exists(string fileName,bool inProject = false)
        {
            if(inProject) return FileUtility.Exists(fileName.WithProjectDataPath());
            return FileUtility.Exists(fileName.WithPersistentDataPath());
        }
        public static bool Exists<T>(string fileName, out T file)
        {
            bool fileExists = FileUtility.Exists(fileName);
            if (fileExists)
                file = Load<T>(fileName.WithPersistentDataPath());
            else
                file = default(T);
            return fileExists;
        }
        public static bool Delete(string fileName,bool inProject = false)
        {
            if (inProject) return FileUtility.DeleteAt(fileName.WithProjectDataPath());
            return FileUtility.Delete(fileName);
        }
        public static T Load<T>(string fileName)
        {
            string json = File.ReadAllText(fileName.WithPersistentDataPath());
            return JsonUtility.FromJson<T>(json);
        }
        public static string LoadRaw(string fileName,bool fromProject = false)
        {
            if(fromProject) return File.ReadAllText(fileName.WithProjectDataPath());
            return File.ReadAllText(fileName.WithPersistentDataPath());
        }

    }
}

