using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voxelity.Editor;
using Voxelity.Extensions;

namespace Voxelity.Save.Editor
{
    [CustomEditor(typeof(SaveDirectory))]
    public class SaveDirectoryEditor : UnityEditor.Editor
    {
        SavableInfo.ValueTypes d_type;
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
        public void OnEnable()
        {
            if(target != null)
                targetObject.Refresh();
        }
        public override void OnInspectorGUI()
        {
            if (ShowLocked()) return;
            if (VoxelityGUI.Button("Lock"))
            {
                targetObject.lockObj = true;
            }
            VoxelityGUI.DisabledGroup(() =>
            {
                base.OnInspectorGUI();
            }, true);

            if (targetObject.saveName == "")
                EditorGUILayout.HelpBox("Set the name of the file to use properties.", MessageType.Warning);

            EditorGUILayout.Space();
            if (VoxelityGUI.InLineButton("Set Save Name", () =>
            {
                d_saveName = EditorGUILayout.TextField(d_saveName);
            }))
            {
                targetObject.saveName = d_saveName;
            }
            VoxelityGUI.DisabledGroup(() =>
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Type",GUILayout.MaxWidth(75));
                targetObject.saveToProject = VoxelityGUI.DisplayActiveToggle(targetObject.saveToProject, new string[2] { "Game", "Project" });
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                DrawAddSection();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal("box");

                if (VoxelityGUI.Button("Delete save file", GUILayout.MaxHeight(25)))
                {
                    if (JsonSaver.Exists(targetObject.saveName , targetObject.saveToProject))
                    {
                        JsonSaver.Delete(targetObject.saveName , targetObject.saveToProject);
                        Debug.Log(targetObject.saveName.Bold() + ":" + " Save file deleted".Colorize(Color.green));
                    }
                    else
                    {
                        Debug.Log(targetObject.saveName.Bold() + ":" + " is not available".Colorize(Color.yellow));
                    }
                }

                if (VoxelityGUI.Button("Delete All", GUILayout.MaxHeight(25)))
                {
                    targetObject.RemoveAll();
                    return;
                }
                if (VoxelityGUI.Button("Save", GUILayout.MaxHeight(25)))
                {
                    targetObject.Save();
                }

                if (VoxelityGUI.Button("Load", GUILayout.MaxHeight(25)))
                {
                    targetObject.Load();
                }
                EditorGUILayout.EndHorizontal();
                DrawItems();
            }, targetObject.saveName == "");
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

            d_type = (SavableInfo.ValueTypes)GUILayout.SelectionGrid((int)d_type, tabNames, 5);
            EditorGUILayout.BeginHorizontal();
            d_name = EditorGUILayout.TextField("Name ", d_name);
            EditorGUILayout.EndHorizontal();
            switch (d_type)
            {
                case SavableInfo.ValueTypes.Integer:
                    d_int = EditorGUILayout.IntField("Value", d_int);
                    EditorGUILayout.Space();
                    DisplayButton(new SaveData<int>(d_name, d_int));
                    break;
                case SavableInfo.ValueTypes.Float:
                    d_float = EditorGUILayout.FloatField("Value", d_float);
                    EditorGUILayout.Space();
                    DisplayButton(new SaveData<float>(d_name, d_float));
                    break;
                case SavableInfo.ValueTypes.Bool:
                    d_bool = EditorGUILayout.Toggle("Value", d_bool);
                    EditorGUILayout.Space();
                    DisplayButton(new SaveData<bool>(d_name, d_bool));
                    break;
                case SavableInfo.ValueTypes.String:
                    var textAreaStyle = new GUIStyle(EditorStyles.textArea) { };
                    textAreaStyle.wordWrap = true;
                    EditorGUILayout.PrefixLabel("Value");
                    d_string = EditorGUILayout.TextArea(d_string, textAreaStyle);
                    EditorGUILayout.Space();
                    if (d_string == "") d_string = "N/A";
                    DisplayButton(new SaveData<string>(d_name, d_string));
                    break;
                case SavableInfo.ValueTypes.Vector3:
                    d_vector3 = EditorGUILayout.Vector3Field("", d_vector3);
                    EditorGUILayout.Space();
                    DisplayButton(new SaveData<Vector3>(d_name, d_vector3));
                    break;
            }
            GUILayout.EndVertical();
        }
        private void DisplayButton<T>(SaveData<T> saved)
        {
            var buttonStyle = new GUIStyle(GUI.skin.button) { fixedHeight = 30 };
            if (GUILayout.Button("Add Save", buttonStyle))
            {
                targetObject.AddSavable(saved);
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
