using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();

            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 };
            EditorGUILayout.LabelField("Value Settings", style);

            EditorGUILayout.BeginHorizontal();
            d_saveName = EditorGUILayout.TextField(d_saveName);
            if(GUILayout.Button("Set Save Name"))
            {
                targetObject.saveName = d_saveName;
            }
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
            d_type = (SavableInfo.ValueTypes)GUILayout.SelectionGrid((int)d_type, tabNames,5);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            d_name = EditorGUILayout.TextField("Name ",d_name);
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
            if (GUILayout.Button("Add Save",buttonStyle))
            {
                targetObject.AddSavable(saved);
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        private void DrawItems()
        {
            foreach (var item in targetObject.Savables)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty serializedPropertyData = serializedObject.FindProperty("saveData");
                EditorGUILayout.Space();
                serializedObject.Update();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedPropertyData.FindPropertyRelative("_value"), new GUIContent("Saved Value :" + item.name), true);
                //EditorGUILayout.LabelField("Default: " + 0); // Write default mb
                EditorGUILayout.EndHorizontal();
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                var buttonStyle = new GUIStyle(GUI.skin.button) { margin = new RectOffset(0, 0, 5, 5) };
                if (GUILayout.Button("Remove",buttonStyle))
                {
                    targetObject.RemoveSavable(item);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
