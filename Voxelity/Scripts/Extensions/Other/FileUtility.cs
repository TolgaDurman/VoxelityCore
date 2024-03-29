﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using UnityEditor;

namespace Voxelity.Extensions.Utility
{
    public static class FileUtility
    {
        private static string PersistentDataPath = Application.persistentDataPath;

        public static void Save(string path, Action<BinaryWriter> onWriter)
        {
            using (FileStream fs = new FileStream(GetFullPath(path), FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                SystemUtility.SafeCall(onWriter, bw);
            }
        }

        public static void Load(string path, Action<BinaryReader> onReader)
        {
            using (FileStream fs = new FileStream(GetFullPath(path), FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                SystemUtility.SafeCall(onReader, br);
            }
        }
        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static bool Delete(string path)
        {
            if (Exists(path))
            {
                File.Delete(GetFullPath(path));
                return true;
            }
            return false;
        }
        public static bool DeleteAt(string path)
        {
            if (Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static IEnumerator LoadStreamingAssets(string path, Action<byte[]> result)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath + path);

#if UNITY_ANDROID && !UNITY_EDITOR
            using (var www = new WWW(fullPath))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }

                SystemUtility.SafeCall(result, www.bytes);
            }
#else
            SystemUtility.SafeCall(result, File.ReadAllBytes(fullPath));
            yield break;
#endif
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static string GetFullPath(string path)
        {
            return Path.Combine(PersistentDataPath, path);
        }
    }
}