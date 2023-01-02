using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voxelity.SO;

namespace Voxelity.Editor
{
    public class VoxelityEditorWindow : EditorWindow
    {
        //[MenuItem("Voxelity/Settings Window", priority = -100)]
        public static void ShowWindow()
        {
            GetWindow<VoxelityEditorWindow>("Voxelity");
        }
        public void OnGUI()
        {
            
        }
    }
}