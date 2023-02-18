using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    [System.Serializable]
    public struct VoxelityTabSetting
    {
        public string name;
        public string toolTip;
        public int priority;
        public Texture2D icon;

        public VoxelityTabSetting(string name, int priority = 0,string toolTip = "", Texture2D icon = null)
        {
            this.name = name;
            this.toolTip = toolTip;
            this.priority = priority;
            this.icon = icon;
        }
    }
}

