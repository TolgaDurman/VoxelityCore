using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Extensions.Utility;
using Voxelity.Editor.Tabs;

namespace Voxelity.Save.Editor
{
    public class SaveSystemTab : VoxelityTab
    {
        private static List<SaveDirectory> cachedSaveDirectories = new List<SaveDirectory>();
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
            cachedSaveDirectories.Clear();
            foreach (var item in Resources.FindObjectsOfTypeAll<SaveDirectory>())
            {
                cachedSaveDirectories.Add(item);
            }
            foldouts = new bool[cachedSaveDirectories.Count];
            editors = new UnityEditor.Editor[cachedSaveDirectories.Count];
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
            },width:60))
            {
                Refresh();
            }
            for (int i = 0; i < cachedSaveDirectories.Count; i++)
            {
                if (cachedSaveDirectories[i] == null)
                {
                    Refresh();
                    EditorGUILayout.EndVertical();
                    return;
                }
                VoxelityGUI.Line();
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], cachedSaveDirectories[i].name);
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
