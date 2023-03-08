using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Voxelity.Extensions;
using System.Reflection;

namespace Voxelity.Editor
{
    public static class VoxelityGUI
    {
        public static string[] BoolNames = new string[2] { "False", "True" };
        public static string[] ActiveNames = new string[2] { "Disabled", "Active" };

        public static GUIStyle BoldLabel(TextAnchor alignment = TextAnchor.MiddleCenter,int fontSize = 12, Color color = default)
        {
            var style = new GUIStyle(GUI.skin.label) 
            { 
                fontStyle = FontStyle.Bold, 
                alignment = alignment, 
                fontSize = fontSize,
            };
            if(color == default)
            {
                color = Color.white;
            }
            style.normal.textColor = color;
            return style;
        }

        public static bool isDarkTheme
        {
            get => EditorGUIUtility.isProSkin;
        }
        public static Color LineColor = new Color(0f, 0f, 0f, 0.1f);

        public static Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.alphaIsTransparency = true;
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        public static void DisabledGroup(Action group, bool condition = true)
        {
            EditorGUI.BeginDisabledGroup(condition);
            group?.Invoke();
            EditorGUI.EndDisabledGroup();
        }
        public static List<T> GetSelectionListAs<T>() where T : UnityEngine.Object
        {
            List<T> returned = new List<T>();
            UnityEngine.Object[] selecteds = Selection.objects;
            foreach (var item in selecteds)
            {
                if (item is T)
                {
                    returned.Add(item as T);
                }
            }
            return returned;
        }

        public static bool Button(string text = "", params GUILayoutOption[] styles)
        {
            return GUILayout.Button(text, styles);
        }
        public static void Button(Action doOn, string text = "", params GUILayoutOption[] styles)
        {
            if (text == "")
                text = doOn.Method.Name;

            if (Button(text, styles))
                doOn?.Invoke();
        }
        public static void DisplayInWindow(Action content,string label = "")
        {
            EditorGUILayout.BeginVertical("window");
            if(label != "")
                VoxelityGUI.Header(label);
            
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }
        public static void DisplayInBox(Action content,string label = "")
        {
            EditorGUILayout.BeginVertical("box");
            if(label != "")
                VoxelityGUI.Header(label);
            
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static bool AskUserDialog(string header, string text, string accepted = "Yes", string declined = "No")
        {
            return EditorUtility.DisplayDialog(header, text, accepted, declined);
        }
        public static void Header(string text, bool useLine = true)
        {
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 };
            EditorGUILayout.LabelField(text, style, GUILayout.ExpandWidth(true));
            if (useLine)
                Line();
        }
        public static void Line(float height = 1f, Color color = default)
        {
            if (color == default) color = LineColor;
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), GetTexture(color));
        }
        public static void ClearSelection()
        {
            Selection.objects = null;
        }
        public static bool DisplayActiveToggle(bool isActiveBase, string[] names = null, bool reverseOrder = false)
        {
            if (names == null)
                names = ActiveNames;

            if (reverseOrder)
            {
                names = names.ToList().Reverse<string>().ToArray();
                return !HorizontalToolbarSelection((!isActiveBase).ToInt(), names).ToBool();
            }
            return HorizontalToolbarSelection(isActiveBase.ToInt(), names).ToBool();
        }
        public static int HorizontalToolbarSelection(int currentValue, string[] tabNames)
        {
            return GUILayout.SelectionGrid(currentValue, tabNames, tabNames.Length);
        }

        public static void ScriptableObjectGUI(ScriptableObject obj, List<string> ignoredProperties = null)
        {
            GUILayout.Space(10f);
            GUIStyle verticalWindowStyle = new GUIStyle("window")
            {
                stretchHeight = false,
            };
            GUILayout.BeginVertical(obj.name, verticalWindowStyle);
            GUILayout.Space(10f);
            SerializedObject serializedObject = new SerializedObject(obj);
            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            bool enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                if (property.name == "m_Script")
                {
                    enterChildren = false;
                    continue;
                }

                if (ignoredProperties != null && ignoredProperties.Contains(property.name))
                {
                    enterChildren = false;
                    continue;
                }
                enterChildren = false;
                EditorGUILayout.PropertyField(property, true);
            }
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
        public static bool InLineButton(string label, Action inLine, bool isLeft = false, params GUILayoutOption[] layoutOptions)
        {
            EditorGUILayout.BeginHorizontal();

            if (!isLeft)
                inLine?.Invoke();

            bool isPressed = Button(label, layoutOptions);

            if (isLeft)
                inLine?.Invoke();

            EditorGUILayout.EndHorizontal();
            return isPressed;
        }
        public static bool InLineButtons(Action inLine, string[] labels, out int pressedIndex, bool isLeft = false, params GUILayoutOption[] layoutOptions)
        {
            EditorGUILayout.BeginHorizontal();
            pressedIndex = -1;

            if (!isLeft)
                inLine?.Invoke();

            for (int i = 0; i < labels.Length; i++)
            {
                if (Button(labels[i], layoutOptions))
                {
                    pressedIndex = i;
                    EditorGUILayout.EndHorizontal();
                    return true;
                }
            }

            if (isLeft)
                inLine?.Invoke();

            EditorGUILayout.EndHorizontal();
            return false;
        }
    }
}
