using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Voxelity.Extensions;

namespace Voxelity.Editor.Tabs
{
    [FilePath("VoxelitySettings/Tabs", FilePathAttribute.Location.PreferencesFolder)]
    public class VoxelitySettings : ScriptableSingleton<VoxelitySettings>
    {
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



        public void OnGUI()
        {
            VoxelityGUI.Header("Tabs", false);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Dark", new GUIStyle(EditorStyles.boldLabel));
            tabBackgroundDark = DisplayColor(tabBackgroundDark, "Tab BG").WithA(1f);
            tabContentDark = DisplayColor(tabContentDark, "Tab Content BG").WithA(1f);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Light", new GUIStyle(EditorStyles.boldLabel));
            tabBackgroundLight = DisplayColor(tabBackgroundLight, "Tab BG").WithA(1f);
            tabContentLight = DisplayColor(tabContentLight, "Tab Content BG").WithA(1f);
            EditorGUILayout.EndVertical();
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
            Save(true);
        }
        private Color DisplayColor(Color color, string name)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(120));
            color = EditorGUILayout.ColorField(color);
            EditorGUILayout.EndHorizontal();
            return color;
        }
    }
}
