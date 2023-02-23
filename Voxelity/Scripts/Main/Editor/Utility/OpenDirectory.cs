#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

namespace Voxelity.Editor
{
    public static class OpenDirectory
    {
        [MenuItem("Voxelity/OpenDir/DataPath")]
        public static void OpenDataPath()
        {
            Process.Start(Application.dataPath);
        }

        [MenuItem("Voxelity/OpenDir/TemporaryCachePath")]
        public static void OpenTemporaryCachePath()
        {
            Process.Start(Application.temporaryCachePath);
        }

        [MenuItem("Voxelity/OpenDir/PersistentDataPath")]
        public static void OpenPersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Voxelity/OpenDir/StreamingAssetsPath")]
        public static void OpenStreamingAssetsPath()
        {
            Process.Start(Application.streamingAssetsPath);
        }

        [MenuItem("Voxelity/OpenDir/ApplicationPath")]
        public static void OpenApplicationPath()
        {
            Process.Start(EditorApplication.applicationPath);
        }

        [MenuItem("Voxelity/OpenDir/ApplicationContentsPath")]
        public static void OpenApplicationContentsPath()
        {
            Process.Start(EditorApplication.applicationContentsPath);
        }
    }
}
#endif