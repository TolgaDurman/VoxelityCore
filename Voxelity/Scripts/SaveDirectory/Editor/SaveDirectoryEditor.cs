using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voxelity.Editor;

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
        SaveDirectory targetObject;
        string d_saveName;
        string[] tabNames = new string[5]
        {
            "Int",
            "String",
            "Float",
            "Bool",
            "Vector3",
        };

        private void OnEnable()
        {
            targetObject = (SaveDirectory)target;
        }
        private bool ShowLocked()
        {
            if (targetObject.lockObj)
            {
                VoxelityGUI.Header("LOCKED");
                Rect textureRect = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
                textureRect.x = (EditorGUIUtility.currentViewWidth - 256) / 2;
                GUI.DrawTexture(textureRect, EditorGUIUtility.FindTexture("Assets/Voxel Studio/Voxelity/Icons/Lock.png"));
                if (VoxelityGUI.Button("Unlock", 25))
                {
                    targetObject.lockObj = false;
                }
            }
            return targetObject.lockObj;
        }
        public override void OnInspectorGUI()
        {
            if (ShowLocked()) return;
            if (GUILayout.Button("Lock"))
            {
                targetObject.lockObj = true;
            }
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();

            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 };
            EditorGUILayout.LabelField("Value Settings", style);
            if (targetObject.saveName == "")
                EditorGUILayout.HelpBox("Set the name of the file to use properties.", MessageType.Warning);
            
            EditorGUILayout.BeginHorizontal();

            d_saveName = EditorGUILayout.TextField(d_saveName);
            if (GUILayout.Button("Set Save Name"))
            {
                targetObject.saveName = d_saveName;
            }
            if (targetObject.saveName == "")
                EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            DrawSavables();

            EditorGUILayout.Space();

            if (GUILayout.Button("Delete All"))
            {
                targetObject.RemoveAll();
                return;
            }
            if (GUILayout.Button("Save"))
            {
                targetObject.Save();
            }

            if (GUILayout.Button("Load"))
            {
                targetObject.Load();
            }
            DrawItems();
        }
        private void DrawSavables()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            d_type = (SavableInfo.ValueTypes)GUILayout.SelectionGrid((int)d_type, tabNames, 5);
            EditorGUILayout.Space();
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
                    DisplayButton(new SaveData<string>(d_name, d_string));
                    break;
                case SavableInfo.ValueTypes.Vector3:
                    d_vector3 = EditorGUILayout.Vector3Field("", d_vector3);
                    EditorGUILayout.Space();
                    DisplayButton(new SaveData<Vector3>(d_name, d_vector3));
                    break;
            }
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
                VoxelityGUI.Header(item.name);
                EditorGUILayout.BeginVertical("box");
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty serializedPropertyData = serializedObject.FindProperty("saveData");
                EditorGUILayout.Space();
                serializedObject.Update();

                EditorGUILayout.PropertyField(serializedPropertyData, new GUIContent("Data : "), true);

                EditorGUILayout.BeginHorizontal();
                var buttonStyle = new GUIStyle(GUI.skin.button) { margin = new RectOffset(0, 0, 5, 5) };
                if (GUILayout.Button("Remove", buttonStyle))
                {
                    targetObject.RemoveSavable(item);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

    }
}
