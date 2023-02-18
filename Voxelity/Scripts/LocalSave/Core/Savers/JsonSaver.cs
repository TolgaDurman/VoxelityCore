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
            if(!FileUtility.Exists(SavableInfo.SavePath))
                FileUtility.CreateFolder(SavableInfo.SavePath);
        }
        private const string c_Key ="JsonSaversaves";
        private static string GetPassword()
        {
            if(!PlayerPrefs.HasKey(c_Key))
            {
                PlayerPrefs.SetString(c_Key, CryptUtility.CreatePassword(16));
            }
            return PlayerPrefs.GetString(c_Key);
        }
        public static void Save<T>(string fileName, T data)
        {
            string json ="";
            json = JsonUtility.ToJson(data);
            File.WriteAllText(fileName.WithPersistentSaveDataPath(), json);
        }
        public static void SaveRaw(string fileName, string data)
        {
            File.WriteAllText(fileName.WithPersistentSaveDataPath(), data);
        }
        public static void SaveCrypted<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data);

            string encrypted;

            CryptUtility.EncryptAESWithECB(json, out encrypted, GetPassword());

            FileUtility.Save(fileName, bw=>
            {
                bw.Write(encrypted);
            });
        }
        public static void SaveCrypted<T>(string fileName, T data,string key)
        {
            string json = JsonUtility.ToJson(data);

            string encrypted;

            CryptUtility.EncryptAESWithECB(json, out encrypted, key);

            FileUtility.Save(fileName, bw=>
            {
                bw.Write(encrypted);
            });
        }

        public static T LoadCrypted<T>(string fileName)
        {
            string json = File.ReadAllText(fileName.WithPersistentSaveDataPath());

            string decrypted;

            CryptUtility.DecryptAESWithECB(json, out decrypted, GetPassword());

            return JsonUtility.FromJson<T>(decrypted);
        }
        public static T LoadCrypted<T>(string fileName,string key)
        {
            string json = File.ReadAllText(fileName.WithPersistentSaveDataPath());

            string decrypted;

            CryptUtility.DecryptAESWithECB(json, out decrypted, key);

            return JsonUtility.FromJson<T>(decrypted);
        }
        public static bool Exists(string fileName)
        {
            return FileUtility.Exists(fileName);
        }
        public static bool Exists<T>(string fileName, out T file)
        {
            bool fileExists = FileUtility.Exists(fileName);
            if (fileExists)
                file = Load<T>(fileName.WithPersistentSaveDataPath());
            else
                file = default(T);
            return fileExists;
        }
        public static bool Delete(string fileName)
        {
            return FileUtility.Delete(fileName);
        }
        public static T Load<T>(string fileName)
        {
            string json = File.ReadAllText(fileName.WithPersistentSaveDataPath());
            return JsonUtility.FromJson<T>(json);
        }
        public static string LoadRaw(string fileName)
        {
            return File.ReadAllText(fileName.WithPersistentSaveDataPath());
        }

    }
}

