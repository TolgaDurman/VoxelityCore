using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voxelity.DataPacks.SaveDir.Editor
{
    [CustomEditor(typeof(Savable<>),true)]
    public class SavableBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}
