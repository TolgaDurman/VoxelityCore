using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    [VTab]
    public abstract class Tab : ITab
    {
        public virtual void Init() { }
        public virtual void OnSelected() { }
        public virtual void OnDeselected() { }
        public abstract void OnGUI();
        public abstract TabData TabInfo();
    }
}
