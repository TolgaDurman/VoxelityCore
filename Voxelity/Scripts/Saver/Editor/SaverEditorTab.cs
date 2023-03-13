using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelity.Editor.Tabs;

namespace Voxelity.Saver.Editor
{
    public class SaverEditorTab : VoxelityTab
    {
        VoxelityTabSetting _settings = new VoxelityTabSetting("Save");
        public override void OnGUI()
        {

        }

        public override VoxelityTabSetting TabSettings()
        {
            return _settings;
        }
    }
}
