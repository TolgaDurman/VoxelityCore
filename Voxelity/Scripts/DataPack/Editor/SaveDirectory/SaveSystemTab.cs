using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Extensions.Utility;
using Voxelity.Editor.Tabs;
using System.Diagnostics;

namespace Voxelity.DataPacks.SaveDir.Editor
{
    public class SaveSystemTab : VoxelityTab
    {
        WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor> itemsDisplay =
        new WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor>(true, true, "LevelSaves", "ProjectSettings");

        private SaveDirectory[] GetSaveDirectories
        {
            get => Resources.FindObjectsOfTypeAll<SaveDirectory>();
        }

        private VoxelityTabSetting tabSettings = new VoxelityTabSetting("Saves", -101);
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            // JsonSaver.Init();
        }
        public override void OnSelected()
        {
            itemsDisplay.Init(GetSaveDirectories);
        }

        public override void OnGUI()
        {
            if (VoxelityGUI.Button("Open game saves path", GUILayout.Height(30)))
                OpenDirectory.OpenPersistentDataPath();
            
            VoxelityGUI.DisplayInBox(() =>
            {
                if (VoxelityGUI.InLineButton("Refresh", () =>
                            {
                                VoxelityGUI.Header("Save Directories in Resources",false);
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
                        return;
                    }
                    if (itemsDisplay.Foldout(item))
                    {
                        itemsDisplay.DisplayItem(itemsDisplay.IndexOf(item));
                    }
                });
            });
        }

        public override VoxelityTabSetting TabSettings()
        {
            return tabSettings;
        }
    }
}
