using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voxelity.Editor;

namespace Voxelity.DataPacks.Editor
{

    [CustomEditor(typeof(SavableDataObjectBase), true)]
    public class SavableDataObjectBaseEditor : UnityEditor.Editor
    {
        SavableDataObjectBase targetObj;
        private void OnEnable()
        {
            targetObj = (SavableDataObjectBase)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (VoxelityGUI.Button("Save",GUILayout.Height(50)))
            {
                targetObj.Save();
            }
            if (VoxelityGUI.Button("Load",GUILayout.Height(50)))
            {
                targetObj.Load();
            }
        }
    }
}
