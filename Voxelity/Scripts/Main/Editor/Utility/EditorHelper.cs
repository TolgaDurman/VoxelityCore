using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Voxelity.Editor
{
    public static class EditorHelper
    {
        private const string defineName = "VOXELITY_CORE";
#if !VOXELITY_CORE
        [InitializeOnLoadMethod]
        private static void AddDefine()
        {
            AddSymbols(defineName);
        }
#endif
        public static void AddSymbols(params string[] args)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(args.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }

        public static void RemoveSymbols(params string[] args)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.RemoveAll(x => args.Any(y => x == y));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }
        public static void DeleteVisibleFilesAndFolders(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);

            // Delete all visible files in the directory
            foreach (FileInfo file in dir.GetFiles())
            {
                if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    file.Delete();
                }
            }

            // Delete all visible folders in the directory
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if ((subDir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    subDir.Delete(true);
                }
            }
        }
        public static T GetOrCreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            var so = AssetDatabase.LoadAssetAtPath<T>(path);
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(so, path);
                AssetDatabase.SaveAssets();
            }
            return so;
        }

        public static void PingObject(string path)
        {
            Object obj = Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(obj);
        }


        public static string GetPath(string path, bool isObject = false)
        {
            string[] folderPaths = path.Split('/');

            string _path = folderPaths[0];

            if (isObject)
                folderPaths[folderPaths.Length - 1] = "";

            for (int i = 1; i < folderPaths.Length; i++)
            {
                _path = CreateFolder(_path, folderPaths[i]);
            }
            return _path;
        }
        public static string CreateFolder(string path, string folderName)
        {
            if (folderName == "") return path;

            if (!AssetDatabase.IsValidFolder(path + "/" + folderName))
                AssetDatabase.CreateFolder(path, folderName);

            return path + "/" + folderName;
        }
        public static List<T> FindAssets<T>(params string[] paths) where T : UnityEngine.Object
        {
            string[] assetGUIDs = AssetDatabase.FindAssets("t:" + typeof(T), paths);
            List<T> assets = new List<T>();
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                assets.Add(asset);
            }
            return assets;
        }
    }
}