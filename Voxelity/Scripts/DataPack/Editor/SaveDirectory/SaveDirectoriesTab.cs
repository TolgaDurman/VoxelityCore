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
    public class SaveDirectoriesTab : Tab
    {
        WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor> itemsDisplay =
        new WindowEditorDisplay<SaveDirectory, SaveDirectoryEditor>(true, true, "LevelSaves", "ProjectSettings");

        private SaveDirectory[] GetSaveDirectories
        {
            get => Resources.FindObjectsOfTypeAll<SaveDirectory>();
        }

        private TabData tabData = new TabData("Save Directories", -999,"",EditorGUIUtility.IconContent("d_Profiler.NetworkOperations").image);

        public override void OnSelected()
        {
            itemsDisplay.Init(GetSaveDirectories);
        }

        public override void OnGUI()
        {
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

        public override TabData TabInfo()
        {
            return tabData;
        }
    }
}
