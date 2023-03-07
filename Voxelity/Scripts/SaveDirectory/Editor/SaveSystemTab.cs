using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Extensions.Utility;
using Voxelity.Editor.Tabs;
using System.Diagnostics;

namespace Voxelity.Save.Editor
{
    public class SaveSystemTab : VoxelityTab
    {
        private SaveDirectory[] cachedSaveDirectories;
        private bool[] foldouts;
        private UnityEditor.Editor[] editors;
        public string[] ignoredNames = new string[]
        {
            "LevelSaves",
            "ProjectSettings",
        };

        private VoxelityTabSetting tabSettings = new VoxelityTabSetting("Saves", -101);
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            JsonSaver.Init();
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
                item.Refresh();
                
                if (ArrayUtility.Contains(ignoredNames, item.name)) continue;

                ArrayUtility.Add(ref cachedSaveDirectories, item);
            }
            foldouts = new bool[cachedSaveDirectories.Length];
            editors = new UnityEditor.Editor[cachedSaveDirectories.Length];
        }

        public override void OnGUI()
        {
            if (VoxelityGUI.Button("Open game saves path", GUILayout.Height(30)))
                OpenDirectory.OpenPersistentDataPath();
            if (VoxelityGUI.Button("Open project saves path", GUILayout.Height(30)))
            {
                if (!FileUtility.Exists(SavableInfo.SaveToProjectPath))
                {
                    FileUtility.CreateFolder(SavableInfo.SaveToProjectPath);
                }
                Process.Start(SavableInfo.SaveToProjectPath);
            }
            if (VoxelityGUI.Button("Delete all saves", GUILayout.Height(30)))
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
            }, layoutOptions: GUILayout.Width(55)))
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
                }, false, GUILayout.Width(20), GUILayout.Height(13)))
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
