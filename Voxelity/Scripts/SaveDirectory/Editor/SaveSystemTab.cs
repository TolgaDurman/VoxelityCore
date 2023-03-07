using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Extensions.Utility;
using Voxelity.Editor.Tabs;
using System.Diagnostics;
using System.Linq;

namespace Voxelity.Save.Editor
{
    public class SaveSystemTab : VoxelityTab
    {
        WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor> itemsDisplay = 
        new WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor>(true,true,"LevelSaves","ProjectSettings");

        private SaveDirectory[] GetSaveDirectories
        {
            get => Resources.FindObjectsOfTypeAll<SaveDirectory>();
        }

        private VoxelityTabSetting tabSettings = new VoxelityTabSetting("Saves", -101);
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            JsonSaver.Init();
        }
        public override void OnSelected()
        {
            itemsDisplay.Init(GetSaveDirectories);
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
                itemsDisplay.Init(GetSaveDirectories);
            }
            itemsDisplay.CustomDraw((item) =>
            {
                if (VoxelityGUI.InLineButton("X", () =>
                {
                    itemsDisplay.DisplayFoldout(itemsDisplay.IndexOf(item));
                }, false, GUILayout.Width(20), GUILayout.Height(13)))
                {
                    item.RemoveAll();
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item));
                    AssetDatabase.Refresh();
                    itemsDisplay.Init(GetSaveDirectories);
                }
                if(itemsDisplay.Foldout(item))
                {
                    itemsDisplay.DisplayItem(itemsDisplay.IndexOf(item));
                }
            });
            EditorGUILayout.EndVertical();
        }

        public override VoxelityTabSetting TabSettings()
        {
            return tabSettings;
        }
    }
}
