using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Extensions.Utility;
using Voxelity.Editor.Tabs;
using System.Threading;

namespace Voxelity.Save.Editor
{
    public class SaveSystemTab : VoxelityTab
    {
        private SaveDirectory[] cachedSaveDirectories;
        private bool[] foldouts;
        private UnityEditor.Editor[] editors;

        private VoxelityTabSetting tabSettings = new VoxelityTabSetting("Saves", -101);
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            if (!FileUtility.Exists(SavableInfo.SavePath))
            {
                FileUtility.CreateFolder(SavableInfo.SavePath);
            }
        }
        public override void OnSelected()
        {
            Refresh();
        }
        private void Refresh()
        {
            cachedSaveDirectories = new SaveDirectory[0];
            foreach (var item in Resources.FindObjectsOfTypeAll<SaveDirectory>())
            {
                if (item.name != "LevelSaves")
                    ArrayUtility.Add(ref cachedSaveDirectories, item);
            }
            foldouts = new bool[cachedSaveDirectories.Length];
            editors = new UnityEditor.Editor[cachedSaveDirectories.Length];
        }

        public override void OnGUI()
        {
            VoxelityGUI.Header("Save Helper");
            if (VoxelityGUI.Button("Open save path", 30))
                OpenDirectory.OpenPersistentDataPath();
            if (VoxelityGUI.Button("Delete all saves", 30))
            {
                if (VoxelityGUI.AskUserDialog("Delete all save files", "Are you sure?"))
                {
                    if (Directory.Exists(SavableInfo.SavePath))
                    {
                        Directory.Delete(SavableInfo.SavePath, true);
                    }
                }
            }
            EditorGUILayout.BeginVertical("box");
            if (VoxelityGUI.InLineButton("Refresh", () =>
            {
                VoxelityGUI.Header("Save Directories in Resources", false);
            }, width: 60))
            {
                Refresh();
            }

            for (int i = 0; i < cachedSaveDirectories.Length; i++)
            {
                if (cachedSaveDirectories[i] == null)
                {
                    Refresh();
                    EditorGUILayout.EndVertical();
                    AssetDatabase.Refresh();
                    return;
                }
                VoxelityGUI.Line();
                if (VoxelityGUI.InLineButton("X", () =>
                {
                    foldouts[i] = EditorGUILayout.Foldout(foldouts[i], cachedSaveDirectories[i].name, toggleOnLabelClick: true);
                },width:20,height:13))
                {
                    cachedSaveDirectories[i].RemoveAll();
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cachedSaveDirectories[i]));
                    Refresh();
                    AssetDatabase.Refresh();
                    break;
                }
                if (foldouts[i])
                {
                    UnityEditor.Editor.CreateCachedEditor(cachedSaveDirectories[i], typeof(SaveDirectoryEditor), ref editors[i]);
                    editors[i].OnInspectorGUI();
                }
            }
            EditorGUILayout.EndVertical();
        }

        public override VoxelityTabSetting TabSettings()
        {
            return tabSettings;
        }
    }
}
