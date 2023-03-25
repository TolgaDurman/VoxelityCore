using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxelity.Editor.Tabs
{
    public interface ITab
    {
        public void Init();
        public void OnSelected();
        public void OnDeselected();
        public void OnGUI();
        public TabData TabInfo();
    }
}
