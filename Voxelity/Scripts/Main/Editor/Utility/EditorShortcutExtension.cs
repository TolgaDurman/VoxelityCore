using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Voxelity.Editor
{
    public class EditorShortcutExtension
    {
        #if UNITY_EDITOR_2021_1_OR_NEWER
        //Only works in one column layout.
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            Event e = Event.current;
            if(e == null || !e.shift) return false;

            Object obj = EditorUtility.InstanceIDToObject(instanceId);
            string path = AssetDatabase.GetAssetPath(obj);
            if(AssetDatabase.IsValidFolder(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            return true;
        }
        #endif
    }
}
