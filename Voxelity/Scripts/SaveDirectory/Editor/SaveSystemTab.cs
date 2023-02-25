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
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            if (!FileUtility.Exists(SavableInfo.SavePath))
            {
                FileUtility.CreateFolder(SavableInfo.SavePath);
            }
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
        }

        public override VoxelityTabSetting TabSettings()
        {
            return new VoxelityTabSetting("Saves", -101);
        }
    }
}
