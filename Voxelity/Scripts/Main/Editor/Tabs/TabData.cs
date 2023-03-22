using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    [System.Serializable]
    public struct TabData
    {
        public string name;
        public string toolTip;
        public int priority;
        public Texture icon;

        public TabData(string name, int priority = 0,string toolTip = "", Texture icon = null)
        {
            this.name = name;
            this.toolTip = toolTip;
            this.priority = priority;
            this.icon = icon;
        }
    }
}

