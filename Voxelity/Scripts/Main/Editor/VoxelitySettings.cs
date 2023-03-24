using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Voxelity.Extensions;

namespace Voxelity.Editor
{
    [FilePath("VoxelitySettings/Tabs", FilePathAttribute.Location.PreferencesFolder)]
    public class VoxelitySettings : ScriptableSingleton<VoxelitySettings>
    {
        #region Tab Settings
        public static bool FastRepaint
        {
            get => EditorPrefs.GetBool("FastRepaintVoxelity", false);
            set => EditorPrefs.SetBool("FastRepaintVoxelity", value);
        }
        public Color TabColor
        {
            get => VoxelityGUI.isDarkTheme ? tabBackgroundDark : tabBackgroundLight;
        }
        public Color TabContentColor
        {
            get => VoxelityGUI.isDarkTheme ? tabContentDark : tabContentLight;
        }
        public Color tabBackgroundDark = new Color(0.1568628f, 0.1568628f, 0.1568628f);
        public Color tabContentDark = new Color(0.22f, 0.22f, 0.22f);
        public Color tabBackgroundLight = new Color(0.66f, 0.66f, 0.66f);
        public Color tabContentLight = new Color(0.7843138f, 0.7843138f, 0.7843138f);

        private void ResetColors()
        {
            tabBackgroundDark = new Color(0.1568628f, 0.1568628f, 0.1568628f);
            tabContentDark = new Color(0.22f, 0.22f, 0.22f);
            tabBackgroundLight = new Color(0.66f, 0.66f, 0.66f);
            tabContentLight = new Color(0.7843138f, 0.7843138f, 0.7843138f);
        }
        private void TabSettings()
        {
            VoxelityGUI.DisplayInBox(() =>
            {
                EditorGUILayout.LabelField("Dark", new GUIStyle(EditorStyles.boldLabel));
                tabBackgroundDark = VoxelityGUI.DisplayColor(tabBackgroundDark, "Tab BG").WithA(1f);
                tabContentDark = VoxelityGUI.DisplayColor(tabContentDark, "Tab Content BG").WithA(1f);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Light", new GUIStyle(EditorStyles.boldLabel));
                tabBackgroundLight = VoxelityGUI.DisplayColor(tabBackgroundLight, "Tab BG").WithA(1f);
                tabContentLight = VoxelityGUI.DisplayColor(tabContentLight, "Tab Content BG").WithA(1f);
                FastRepaint = EditorGUILayout.Toggle("Fast Repaint", FastRepaint);
                if (VoxelityGUI.InLineButton("Reset", () =>
                {
                    EditorGUILayout.Space();
                }, false, GUILayout.Width(50)))
                {
                    if (VoxelityGUI.AskUserDialog("Reset colors", "Do you want to reset tab colors to default?"))
                    {
                        ResetColors();
                    }
                }
            }, "Tab Settings");
        }
        #endregion

        #region Log Settings
        public bool Enabled 
        {
            get => EditorPrefs.GetBool("v=Logs_Enabled", false);
            set => EditorPrefs.SetBool("v=Logs_Enabled", value);
        }
        public bool Log 
        {
            get => EditorPrefs.GetBool("v=Logs_Default", true);
            set => EditorPrefs.SetBool("v=Logs_Default", value);
        }
        public bool LogWarning 
        {
            get => EditorPrefs.GetBool("v=Logs_Warning", true);
            set => EditorPrefs.SetBool("v=Logs_Warning", value);
        }
        public bool LogError 
        {
            get => EditorPrefs.GetBool("v=Logs_Error", true);
            set => EditorPrefs.SetBool("v=Logs_Error", value);
        }
        private void LogSettings()
        {
            VoxelityGUI.DisplayInBox(() =>
            {
                Enabled = EditorGUILayout.Toggle("Enabled",Enabled);
                EditorGUI.BeginDisabledGroup(!Enabled);
                Log = EditorGUILayout.Toggle("Log",Log);
                LogWarning = EditorGUILayout.Toggle("Log Warning",LogWarning);
                LogError = EditorGUILayout.Toggle("Log Error",LogError);
                EditorGUI.EndDisabledGroup();
            }, "Logs");
        }
        #endregion

        public void OnGUI()
        {
            TabSettings();
            LogSettings();
            Save(true);
        }
    }
}
