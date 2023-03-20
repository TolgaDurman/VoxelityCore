using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity;
using Newtonsoft.Json;
using System.Linq;

namespace Voxelity.AddOns.Editor
{
    public static class AddOnsUtility
    {
        public const string InfoName = ".txt";
        public static readonly string AddOnsPath = "Packages/co.voxelstudio.voxelity/AddOns~";
        public static readonly string AddOnsPathImported = "Assets/Voxel Studio/Add-Ons";
        public static readonly string AddOnsFullPath = Application.dataPath.TrimEnd("Assets".ToCharArray()) + AddOnsPath;
        public static readonly string AddOnsFullPathImported = Application.dataPath.TrimEnd("Assets".ToCharArray()) + AddOnsPathImported;

        public static string[] GetAvailableFolders()
        {
            return Directory.GetDirectories(AddOnsFullPath);
        }
        public static string[] GetImportedFolders()
        {
            return Directory.GetDirectories(AddOnsFullPathImported);
        }
        public static string[] GetAvailableFolderNames()
        {
            string[] directories = Directory.GetDirectories(AddOnsFullPath);
            for (int i = 0; i < directories.Length; i++)
            {
                directories[i] = Path.GetFileName(directories[i]);
            }
            return directories;
        }
        public static string[] GetImportedFolderNames()
        {
            string[] directories = Directory.GetDirectories(AddOnsFullPathImported);
            for (int i = 0; i < directories.Length; i++)
            {
                directories[i] = Path.GetFileName(directories[i]);
            }
            return directories;
        }

        public static AddOnInfo[] GetAvaliablePacks()
        {
            AddOnInfo[] availableAddOns = new AddOnInfo[GetAvailableFolders().Length];
            for (int i = 0; i < availableAddOns.Length; i++)
            {
                availableAddOns[i] = GetAddOnInfoAt(GetAvailableFolderNames()[i],GetAvailableFolders()[i]);
            }
            return availableAddOns;
        }
        public static AddOnInfo[] GetImportedPacks()
        {
            AddOnInfo[] importedAddOns = new AddOnInfo[GetImportedFolders().Length];
            for (int i = 0; i < importedAddOns.Length; i++)
            {
                importedAddOns[i] = GetAddOnInfoAt(GetImportedFolderNames()[i],GetImportedFolders()[i]);
            }
            return importedAddOns;
        }

        public static AddOnInfo DisplayAddOnInfo(AddOnInfo info)
        {
            info.packName = EditorGUILayout.TextField("Pack Name ", info.packName);
            Vector3Int version = new Vector3Int(info.version.Major, info.version.Minor, info.version.Patch);
            version = EditorGUILayout.Vector3IntField("Version", version);
            info.version = new AddOnVersion(version.x, version.y, version.z);
            EditorGUILayout.LabelField("Description");
            info.description = EditorGUILayout.TextArea(info.description, new GUIStyle(EditorStyles.textArea) { wordWrap = true });
            EditorGUILayout.LabelField(info.packPath);
            info.interactiveImport = EditorGUILayout.Toggle("Interactive import", info.interactiveImport);
            return info;
        }
        public static void ImportPackage(AddOnInfo info)
        {
            string packagePath = Path.Combine(info.packPath, info.packName + ".unitypackage");
            AssetDatabase.ImportPackage(packagePath, info.interactiveImport);
        }
        public static void ImportPackageSilent(AddOnInfo info)
        {
            string packagePath = Path.Combine(info.packPath, info.packName + ".unitypackage");
            AssetDatabase.ImportPackage(packagePath, false);
        }

        public static string ToVersionString(this AddOnVersion self)
        {
            return $"v{self.Major}.{self.Minor}.{self.Patch}";
        }
        public static string GetDataInfoPath(string packName,string path)
        {
            return Path.Combine(path, packName+InfoName);
        }
        public static void SetDataInfo(AddOnInfo info, string infoPath)
        {
            if (File.Exists(infoPath))
                File.Delete(infoPath);
            var sw = File.Create(infoPath);
            sw.Close();
            sw.Dispose();
            File.WriteAllText(infoPath, JsonConvert.SerializeObject(info, Formatting.Indented));
        }
        public static bool AddOnInfoExistsAt(string packName,string path)
        {
            if (File.Exists(Path.Combine(path, (packName+InfoName)))) return true;
            return false;
        }
        public static AddOnInfo GetAddOnInfoAt(string packName,string path)
        {
            var text = File.ReadAllText(Path.Combine(path,packName+ InfoName));
            return JsonConvert.DeserializeObject<AddOnInfo>(text);
        }
        public static string GetInfoToString(AddOnInfo info)
        {
            return JsonConvert.SerializeObject(info, Formatting.Indented);
        }
        public static AddOnInfo GetStringToAddOn(string text)
        {
            return JsonConvert.DeserializeObject<AddOnInfo>(text);
        }
    }
}
