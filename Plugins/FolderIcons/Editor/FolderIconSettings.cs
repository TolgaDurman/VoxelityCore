using System;
using UnityEditor;
using UnityEngine;

namespace Voxelity.Plugins.FolderIcons
{
    [CreateAssetMenu (fileName = "Folder Icon Manager", menuName = "Voxelity/Scriptables/Other/Folder Manager")]
    public class FolderIconSettings : ScriptableObject
    {
        [Serializable]
        public class FolderIcon
        {
            public DefaultAsset folder;

            public Texture2D folderIcon;
            public Texture2D overlayIcon;
        }

        //Global Settings
        public bool showOverlay = true;

        public bool showCustomFolder = true;

        public FolderIcon[] icons = new FolderIcon[0];
    }
}