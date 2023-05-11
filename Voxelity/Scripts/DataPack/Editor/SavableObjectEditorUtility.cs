using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voxelity.DataPacks.Editor
{
    public class SavableObjectEditorUtility
    {
        [MenuItem("Voxelity/Reset Savable Datas")]
        public static void ResetToDefaultValues()
        {
            // Find all ScriptableObjects of a specific type in the project
            string[] guids = AssetDatabase.FindAssets("t:SavableDataObjectBase");
            List<SavableDataObjectBase> scriptableObjects = new List<SavableDataObjectBase>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SavableDataObjectBase scriptableObject = AssetDatabase.LoadAssetAtPath<SavableDataObjectBase>(path);
                scriptableObjects.Add(scriptableObject);
            }

            // Loop through the ScriptableObjects and reset their values
            foreach (SavableDataObjectBase scriptableObject in scriptableObjects)
            {
                scriptableObject.SetToDefaultValue();
            }

            // Save the changes made to the ScriptableObjects
            AssetDatabase.SaveAssets();
        }

    }
}
