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
        private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();

        public static bool isDarkTheme
        {
            get => EditorGUIUtility.isProSkin;
        }

        public static Texture2D GetTexture(Color color)
        {
            if (textureCache.TryGetValue(color, out var cachedTexture))
            {
                if (cachedTexture == null)
                    textureCache.Remove(color);
                else
                    return cachedTexture;
            }

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            textureCache.Add(color, texture);
            return texture;
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

        public static bool Button(string text = "", int height = 15)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = height,
            };
            return GUILayout.Button(text, style);
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
        public static void Line()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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
                return !HorizontalToolbarButtons((!isActiveBase).ToInt(), names).ToBool();
            }
            return HorizontalToolbarButtons(isActiveBase.ToInt(), names).ToBool();
        }
        public static int HorizontalToolbarButtons(int currentValue, string[] tabNames)
        {
            return GUILayout.SelectionGrid(currentValue, tabNames, tabNames.Length);
        }

        public static void ScriptableObjectGUI(ScriptableObject obj, List<string> ignoredProperties = null)
        {
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
            serializedObject.ApplyModifiedProperties();
        }
        public static void ScriptableSingletonObjectGUI<T>(UnityEngine.Object obj, List<string> ignoredProperties = null) where T : ScriptableSingleton<T>
        {
            T instance = ScriptableSingleton<T>.instance;
            SerializedObject serializedObject = new SerializedObject(instance);
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
            serializedObject.ApplyModifiedProperties();
        }
        public static bool InLineButton(string label, Action inLine, bool isLeft = false, float width = 0, float height = 0)
        {
            bool isPressed = false;
            EditorGUILayout.BeginHorizontal();
            if (isLeft)
            {
                inLine?.Invoke();
            }

            if (height == 0 && width == 0)
            {
                if (GUILayout.Button(label))
                    isPressed = true;
            }
            else if (height == 0)
            {
                if (GUILayout.Button(label, GUILayout.Width(width)))
                    isPressed = true;
            }
            else if (width == 0)
            {
                if (GUILayout.Button(label, GUILayout.Height(height)))
                    isPressed = true;
            }
            else
            {
                if (GUILayout.Button(label, GUILayout.Width(width), GUILayout.Height(height)))
                    isPressed = true;
            }

            if (!isLeft)
            {
                inLine?.Invoke();
            }

            EditorGUILayout.EndHorizontal();
            return isPressed;
        }
    }
}
