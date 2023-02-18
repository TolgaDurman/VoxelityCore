using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    public class WindowColorSettingsTab : VoxelityTab
    {
        VoxelityTabSetting mySetting = new VoxelityTabSetting("Tab Settings", 100);
        public override void OnGUI()
        {
            VoxelityTabsColorSettings.instance.OnGUI();
        }
        public override VoxelityTabSetting TabSettings()
        {
            return mySetting;
        }
    }
}
