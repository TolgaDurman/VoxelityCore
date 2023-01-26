using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Voxelity.Extensions;

namespace Voxelity.Editor
{
    public class VoxelityEditorWindow : EditorWindow
    {
        public delegate void OnVoxelityGUI();
        private List<VoxelitySettingTab> displayGuis = new List<VoxelitySettingTab>();

        private int tabs = 0;
        private List<string> tabOptions = new List<string>();

        private float tabWidth = 100;
        private bool isDragging;
        Vector2 scrollPos = Vector2.zero;


        [MenuItem("Voxelity/Settings Window", priority = -100)]
        public static void ShowWindow()
        {
            VoxelityEditorWindow window = GetWindow<VoxelityEditorWindow>("Voxelity");
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>(EditorGUIUtility.isProSkin ? 
            "Assets/Voxel Studio/Voxelity/Icons/Voxel Icon/voxel_logo_green.png" :
            "Assets/Voxel Studio/Voxelity/Icons/Voxel Icon/voxel_logo_black.png");
            GUIContent titleContent = new GUIContent("Voxelity", icon);
            window.titleContent = titleContent;
        }

        public void OnEnable()
        {
            tabOptions.Clear();
            displayGuis.Clear();

            MethodInfo[] methodInfos = ReflectionUtility.GetMethodsWith<VoxelitySettingsAttribute>();
            methodInfos.ForEach(x => 
            {
                var attr = x.GetCustomAttribute<VoxelitySettingsAttribute>();
                object obj = Activator.CreateInstance(x.DeclaringType);
                OnVoxelityGUI setting = (OnVoxelityGUI)x.CreateDelegate(typeof(OnVoxelityGUI), obj);
                displayGuis.Add(new VoxelitySettingTab(attr.Name,attr.Description,attr.Priority,attr.Color,setting));

            });
            ReOrderTabs();
        }

        private void ReOrderTabs()
        {
            displayGuis = displayGuis.OrderBy(x => x.priority).ToList();
            displayGuis.ForEach(x => tabOptions.Add(x.name));
        }
        public void OnGUI()
        {
            Color backgroundColor;

            if(EditorGUIUtility.isProSkin)
                "#282828".TryParseToColor(out backgroundColor);
            else
                "#AEAEAE".TryParseToColor(out backgroundColor);

            float handleRectSize = 3f;

            // Create a new style for the tabs
            var tabStyle = new GUIStyle(EditorStyles.toolbarButton)
            {
                fixedWidth = tabWidth + handleRectSize,
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                fixedHeight = 30,
                margin = new RectOffset(0, 0, 0, 2),
            };

            // Begin a horizontal split rect for resizable layou
            GUILayout.BeginHorizontal();
            // Begin a vertical layout for the tabs
            var backgroundRect = new Rect(0, 0, tabWidth, position.height);

            GUI.DrawTexture(backgroundRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, true, 0, backgroundColor, 0, 0);

            //Draw selection grid on left side
            Rect scrollViewRect = new Rect(0, 0, tabWidth, position.height);

            GUILayout.BeginVertical();
            scrollPos = GUI.BeginScrollView(scrollViewRect, scrollPos, new Rect(0, 0, tabWidth - 20,
            tabOptions.Count * (tabStyle.fixedHeight + tabStyle.margin.bottom)), false, false);
            tabs = GUILayout.SelectionGrid(tabs, tabOptions.ToArray(), 1, tabStyle);
            GUI.EndScrollView();
            GUILayout.EndVertical();

            Rect displayRect = new Rect(tabWidth + handleRectSize + 5f, 0, position.width - tabWidth - 10f, position.height);
            GUILayout.BeginArea(displayRect);
            displayGuis[tabs].gui.Invoke();
            GUILayout.EndArea();
            GUILayout.EndHorizontal();

            // Draw the resize handle
            GUI.color = backgroundColor*2f;
            var resizeHandleRect = new Rect(tabWidth, 0, handleRectSize, position.height);
            var resizeHandleRectInput = new Rect(tabWidth+1f, 0, handleRectSize+2f, position.height);
            GUI.DrawTexture(resizeHandleRect, EditorGUIUtility.whiteTexture);
            EditorGUIUtility.AddCursorRect(resizeHandleRectInput, MouseCursor.ResizeHorizontal);
            GUI.color = Color.white;
            

            // Check for handle drag
            if (Event.current.type == EventType.MouseDown && resizeHandleRectInput.Contains(Event.current.mousePosition))
            {
                isDragging = true;
            }
            if (Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
            }
            if (isDragging)
            {
                tabWidth = Event.current.mousePosition.x;
                tabWidth = Mathf.Clamp(tabWidth, 15, 500);
                Repaint();
            }
        }
        public struct VoxelitySettingTab
        {
            public string name;
            public string description;
            public int priority;

            public Color color;
            public OnVoxelityGUI gui;

            public VoxelitySettingTab(string name, string description, int priority, Color color, OnVoxelityGUI gui)
            {
                this.name = name;
                this.description = description;
                this.priority = priority;
                this.color = color;
                this.gui = gui;
            }
        }
    }
}