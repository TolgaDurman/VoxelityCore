using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Voxelity.Editor;

namespace Voxelity.Editor.Tabs
{
    public class VoxelityTabsEditorWindow : EditorWindow
    {
        static EditorWindow targetWindow;
        private List<VoxelityTab> voxelityTabs = new List<VoxelityTab>();

        private int currentTab = 0;
        private int oldTab = 0;
        private Vector2 tabScrollPos = Vector2.zero;
        private GUIStyle TabStyle
        {
            get
            {
                return new GUIStyle(EditorStyles.miniButtonLeft)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 12,
                    fixedHeight = 30,
                    margin = new RectOffset(1, 0, 0, 0),
                };
            }
        }
        private List<GUIContent> tabContents = new List<GUIContent>();
        private Rect tabRect;
        private Rect contentRect;



        private Vector2 contentScrollPos = Vector2.zero;


        private float tabWidth = 100;
        private bool isDragging;
        float mouseGapOnHandle;
        public static Texture TabWindowIcon
        {
            get => VoxelityGUI.isDarkTheme ?
            AssetDatabase.LoadAssetAtPath<Texture>("Packages/co.voxelstudio.voxelity/Voxelity/Icon/Voxel Icon/voxel_logo_green.png") :
            AssetDatabase.LoadAssetAtPath<Texture>("Packages/co.voxelstudio.voxelity/Voxelity/Icon/Voxel Icon/voxel_logo_black.png");
        }


        [MenuItem("Voxelity/Voxel Tabs %#v", priority = -101)]
        public static void ShowWindow()
        {
            targetWindow = GetWindow<VoxelityTabsEditorWindow>("Voxelity");
            GUIContent titleContent = new GUIContent("Voxelity", TabWindowIcon);
            targetWindow.titleContent = titleContent;
        }
        private void OnEnable()
        {
            if(targetWindow == null) targetWindow = EditorWindow.GetWindow<VoxelityTabsEditorWindow>();
            voxelityTabs.Clear();
            tabContents.Clear();
            InitializeTabs();
            OrderTabs();
            voxelityTabs[currentTab].OnSelected();
        }
        private void InitializeTabs()
        {
            Type[] typesWithTabAttribute = ReflectionUtility.GetTypesWith<TabAttribute>();
            foreach (var type in typesWithTabAttribute)
            {
                if (type.IsSubclassOf(typeof(VoxelityTab)))
                {
                    VoxelityTab voxelityTab = (VoxelityTab)Activator.CreateInstance(type);
                    voxelityTabs.Add(voxelityTab);
                }
            }
        }
        private void OrderTabs()
        {
            voxelityTabs = voxelityTabs.OrderBy(x => x.TabSettings().priority).ToList();
            voxelityTabs.ForEach(x =>
            {
                x.Init();
                tabContents.Add(new GUIContent(x.TabSettings().name, x.TabSettings().icon, x.TabSettings().toolTip));
            });
        }
        public void OnGUI()
        {
            if (voxelityTabs.Count == 0) return;
            DrawTabs();
            DrawContent();
            DrawHandle();
        }

        private void DrawTabs()
        {
            tabRect = new Rect(0, 0, tabWidth, position.height);
            EditorGUI.DrawRect(tabRect, VoxelityTabsColorSettings.instance.TabColor); //Tabs

            GUILayout.BeginArea(tabRect);

            EditorGUILayout.BeginVertical();

            tabScrollPos = EditorGUILayout.BeginScrollView(tabScrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            currentTab = GUILayout.SelectionGrid(currentTab, tabContents.ToArray(), 1, TabStyle);

            if (currentTab != oldTab)
            {
                voxelityTabs[currentTab].OnSelected();
                oldTab = currentTab;
            }

            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            GUILayout.EndArea();
        }
        private void DrawContent()
        {
            contentRect = new Rect(tabWidth, 0, position.width - tabWidth, position.height);

            EditorGUI.DrawRect(contentRect, VoxelityTabsColorSettings.instance.TabContentColor);

            GUILayout.BeginArea(contentRect);
            GUIStyle windowStyle = new GUIStyle("window")
            {
                name = voxelityTabs[currentTab].TabSettings().name,
                fontStyle = FontStyle.Bold,
                fontSize = 15,
        };
            GUILayout.BeginVertical(voxelityTabs[currentTab].TabSettings().name, windowStyle);
            GUILayout.Space(10);
            VoxelityGUI.Line(2f);
            contentScrollPos = EditorGUILayout.BeginScrollView(contentScrollPos, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            voxelityTabs[currentTab].OnGUI();

            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            GUILayout.EndArea();
        }


        private void DrawHandle()
        {
            // Draw the resize handle
            var resizeHandleRect = new Rect(tabWidth, 0, 0, position.height);
            var resizeHandleRectInput = new Rect(tabWidth, 0, 6f, position.height);
            GUI.DrawTexture(resizeHandleRect, VoxelityGUI.GetTexture(Color.black));
            EditorGUIUtility.AddCursorRect(resizeHandleRectInput, MouseCursor.ResizeHorizontal);
            GUI.color = Color.white;

            // Check for handle drag
            if (Event.current.type == EventType.MouseDown && resizeHandleRectInput.Contains(Event.current.mousePosition))
            {
                isDragging = true;
                mouseGapOnHandle = resizeHandleRect.center.x - Event.current.mousePosition.x;
            }
            if (Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
            }
            if (isDragging)
            {
                tabWidth = Event.current.mousePosition.x + mouseGapOnHandle;
                tabWidth = Mathf.Clamp(tabWidth, 50, position.width - 50);
                Repaint();
            }
        }
    }
}
