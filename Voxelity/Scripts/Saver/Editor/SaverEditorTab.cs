using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Editor.Tabs;
using Voxelity.Extensions.Utility;
using Voxelity.Saver.Core.Storage;

namespace Voxelity.Saver.Editor
{
    public class SaverEditorTab : VoxelityTab
    {
        VoxelityTabSetting _settings = new VoxelityTabSetting("Save");
        public override void OnGUI()
        {
            if (VoxelityGUI.Button("Open game saves path", GUILayout.Height(30)))
                OpenDirectory.OpenPersistentDataPath();
            if (VoxelityGUI.Button("Delete All Saves", GUILayout.Height(30)))
                EditorHelper.DeleteVisibleFilesAndFolders(FileAccess.BasePath);
        }

        public override VoxelityTabSetting TabSettings()
        {
            return _settings;
        }
    }
}
