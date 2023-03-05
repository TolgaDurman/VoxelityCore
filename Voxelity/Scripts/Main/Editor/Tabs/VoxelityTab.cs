using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    [Tab]
    public abstract class VoxelityTab
    {
        public virtual void Init() { }
        public virtual void OnSelected() { }
        public abstract void OnGUI();
        public abstract VoxelityTabSetting TabSettings();
    }
}
