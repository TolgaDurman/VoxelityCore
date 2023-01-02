using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Voxelity.SO
{
    public class VoxelitySettings : ScriptableObject
    {
        public VoxelityManager Manager;
        public const string k_VoxelitySettingsPathInPackage = "Packages/co.voxelstudio.voxelity/Voxelity/Resources/VoxelitySettings.asset";
        public const string k_VoxelitySettingsPath = "Assets/Voxelity/Resources/VoxelitySettings.asset";
        public static VoxelitySettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<VoxelitySettings>(k_VoxelitySettingsPathInPackage);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<VoxelitySettings>();
                AssetDatabase.CreateAsset(settings, k_VoxelitySettingsPathInPackage);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerialized()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}