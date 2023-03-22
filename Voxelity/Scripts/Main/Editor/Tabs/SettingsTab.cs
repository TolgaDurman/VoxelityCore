using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voxelity.Editor.Tabs
{
    public class SettingsTab : Tab
    {
        TabData mySetting = new TabData("Settings", 100,"",EditorGUIUtility.IconContent("d__Popup").image);
        public override void OnGUI()
        {
            VoxelitySettings.instance.OnGUI();
        }
        public override TabData TabInfo()
        {
            return mySetting;
        }
    }
}
