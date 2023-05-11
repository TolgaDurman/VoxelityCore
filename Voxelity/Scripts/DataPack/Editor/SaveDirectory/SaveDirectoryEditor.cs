using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voxelity.Editor;
using Voxelity.Extensions;
using System.Linq;

namespace Voxelity.DataPacks.SaveDir.Editor
{
    [CustomEditor(typeof(SaveDirectory))]
    public class SaveDirectoryEditor : UnityEditor.Editor
    {
        DataTypes d_type;
        int d_int;
        float d_float;
        string d_string;
        bool d_bool;
        Vector3 d_vector3;
        string d_name;
        public SaveDirectory targetObject
        {
            get => (SaveDirectory)target;
        }
        string d_saveName;
        string[] tabNames = new string[5]
        {
            "Int",
            "String",
            "Float",
            "Bool",
            "Vector3",
        };
        private bool ShowLocked()
        {
            if (targetObject.lockObj)
            {
                VoxelityGUI.Header("LOCKED");
                if (VoxelityGUI.Button("Unlock", GUILayout.Height(25)))
                {
                    targetObject.lockObj = false;
                }
            }
            return targetObject.lockObj;
        }
        public override void OnInspectorGUI()
        {
            if (HasMatchingName())
            {
                if (VoxelityGUI.InLineButton("Fix now", () =>
                {
                    EditorGUILayout.HelpBox(targetObject.name + ": Has 2 instances with same names. Delete or change one of the Scriptable Objects name. ", MessageType.Error);
                },true,GUILayout.Height(35)))
                {
                    EditorUtility.SetDirty(targetObject);
                    string oldName = targetObject.name;
                    string newName = oldName + "Copy";
                    string assetPath = AssetDatabase.GetAssetPath(targetObject);
                    AssetDatabase.RenameAsset(assetPath, newName);
                    targetObject.name = newName;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            if (ShowLocked()) return;
            if (VoxelityGUI.Button("Lock"))
            {
                targetObject.lockObj = true;
            }
            VoxelityGUI.DisabledGroup(() =>
            {
                base.OnInspectorGUI();
            }, true);

            VoxelityGUI.DisabledGroup(() =>
            {

                EditorGUILayout.Space();

                DrawAddSection();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal("box");

                if (VoxelityGUI.Button("Delete save file", GUILayout.MaxHeight(25)))
                {
                    targetObject.DeleteSaveFile();
                }

                if (VoxelityGUI.Button("Delete All", GUILayout.MaxHeight(25)))
                {
                    targetObject.RemoveAll();
                    return;
                }
                if (VoxelityGUI.Button("Save", GUILayout.MaxHeight(25)))
                {
                    targetObject.SaveAll();
                }

                if (VoxelityGUI.Button("Load", GUILayout.MaxHeight(25)))
                {
                    targetObject.LoadAll();
                }
                EditorGUILayout.EndHorizontal();
                DrawItems();
            }, HasMatchingName());
        }
        private bool HasMatchingName()
        {
            return Resources.FindObjectsOfTypeAll<SaveDirectory>().Any(x =>
            {
                if (!x.Equals(targetObject))
                    return x.name == targetObject.name;
                return false;
            });
        }
        private void DrawAddSection()
        {
            EditorGUILayout.Space();
            var windowStyle = new GUIStyle("window")
            {
                stretchHeight = false,
            };
            windowStyle.fontStyle = FontStyle.Bold;
            GUILayout.BeginVertical("Add Savable", windowStyle);

            d_type = (DataTypes)GUILayout.SelectionGrid((int)d_type, tabNames, 5);
            EditorGUILayout.BeginHorizontal();
            d_name = EditorGUILayout.TextField("Name ", d_name);
            EditorGUILayout.EndHorizontal();
            switch (d_type)
            {
                case DataTypes.Integer:
                    d_int = EditorGUILayout.IntField("Value", d_int);
                    EditorGUILayout.Space();
                    DisplayButton(d_name, d_int);
                    break;
                case DataTypes.Float:
                    d_float = EditorGUILayout.FloatField("Value", d_float);
                    EditorGUILayout.Space();
                    DisplayButton(d_name, d_float);
                    break;
                case DataTypes.Bool:
                    d_bool = EditorGUILayout.Toggle("Value", d_bool);
                    EditorGUILayout.Space();
                    DisplayButton(d_name, d_bool);
                    break;
                case DataTypes.String:
                    var textAreaStyle = new GUIStyle(EditorStyles.textArea) { };
                    textAreaStyle.wordWrap = true;
                    EditorGUILayout.PrefixLabel("Value");
                    d_string = EditorGUILayout.TextArea(d_string, textAreaStyle);
                    EditorGUILayout.Space();
                    if (d_string == "") d_string = "N/A";
                    DisplayButton(d_name, d_string);
                    break;
                case DataTypes.Vector3:
                    d_vector3 = EditorGUILayout.Vector3Field("", d_vector3);
                    EditorGUILayout.Space();
                    DisplayButton(d_name, d_vector3);
                    break;
            }
            GUILayout.EndVertical();
        }
        private void DisplayButton<T>(string _name, T saved)
        {
            var buttonStyle = new GUIStyle(GUI.skin.button) { fixedHeight = 30 };
            if (GUILayout.Button("Add Save", buttonStyle))
            {
                targetObject.AddSavable(new Data<T>(new DataInfo(targetObject.name , _name), saved));
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        private void DrawItems()
        {
            foreach (var item in targetObject.Savables)
            {
                VoxelityGUI.Line();
                EditorGUILayout.BeginVertical("box");
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty serializedPropertyData = serializedObject.FindProperty("value");
                EditorGUILayout.Space();
                serializedObject.Update();

                if (VoxelityGUI.InLineButton("X", () =>
                {
                    EditorGUILayout.PropertyField(serializedPropertyData, new GUIContent(item.name + " : "), true);
                }, false, GUILayout.Width(20)))
                {
                    targetObject.RemoveSavable(item);
                    EditorGUILayout.EndVertical();
                    break;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}
