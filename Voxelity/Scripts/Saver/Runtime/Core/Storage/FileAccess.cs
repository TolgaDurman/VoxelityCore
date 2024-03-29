﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace Voxelity.Saver.Core.Storage
{
    public static class FileAccess
    {
        private const string _defaultExtension = ".json";

        public static string BasePath => Path.Combine(VoxelitySaverGlobalSettings.StorageLocation, "Saves");

        public static bool SaveString(string filename, bool includesExtension, string value)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                using (StreamWriter writer = new StreamWriter(Path.Combine(BasePath, filename)))
                {
                    writer.Write(value);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }
        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        #else
        [RuntimeInitializeOnLoadMethod]
        #endif
        public static void Initialize()
        {
            if(!Directory.Exists(BasePath)) Directory.CreateDirectory(BasePath);
        }

        public static bool SaveBytes(string filename, bool includesExtension, byte[] value)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Create))
                {
                    fileStream.Write(value, 0, value.Length);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static string LoadString(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                if (Exists(filename, true))
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(BasePath, filename)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static byte[] LoadBytes(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                if (Exists(filename, true))
                {
                    using (FileStream fileStream = new FileStream(Path.Combine(BasePath, filename), FileMode.Open))
                    {
                        byte[] buffer = new byte[fileStream.Length];

                        fileStream.Read(buffer, 0, buffer.Length);

                        return buffer;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public static void Delete(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            try
            {
                CreateRootFolder();

                string fileLocation = Path.Combine(BasePath, filename);

                File.Delete(fileLocation);
            }
            catch
            {
                throw new VoxelityException("Directory doesn't exists");
            }
        }

        public static bool Exists(string filename, bool includesExtension)
        {
            filename = GetFilenameWithExtension(filename, includesExtension);

            string fileLocation = Path.Combine(BasePath, filename);

            return File.Exists(fileLocation);
        }

        public static IEnumerable<string> Files(bool includeExtensions)
        {
            try
            {
                CreateRootFolder();

                if (includeExtensions)
                {
                    return Directory.GetFiles(BasePath, "*.json").Select(x => Path.GetFileName(x));
                }
                else
                {
                    return Directory.GetFiles(BasePath, "*.json").Select(x => Path.GetFileNameWithoutExtension(x));
                }
            }
            catch
            {
            }

            return new List<string>();
        }

        private static string GetFilenameWithExtension(string filename, bool includesExtension)
        {
            return includesExtension ? filename : filename + _defaultExtension;
        }

        private static void CreateRootFolder()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }
    }
}