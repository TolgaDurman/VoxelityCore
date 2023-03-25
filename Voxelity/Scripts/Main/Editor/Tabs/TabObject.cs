using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    [VTab]
    public abstract class TabObject : ScriptableObject, ITab
    {
        public virtual void Init() { }

        public virtual void OnDeselected(){}

        public virtual void OnGUI() { }

        public virtual void OnSelected(){}

        public abstract TabData TabInfo();
    }
}
