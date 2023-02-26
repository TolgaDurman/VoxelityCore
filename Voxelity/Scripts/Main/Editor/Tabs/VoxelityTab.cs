using UnityEditor;

namespace Voxelity.Editor.Tabs
{
    [Tab]
    public abstract class VoxelityTab
    {
        public virtual void Init() { }
        public virtual void OnSelected() { }
        public abstract void OnGUI();
        public abstract VoxelityTabSetting TabSettings();
        public virtual void Repaint()
        {
            EditorWindow window = EditorWindow.GetWindow<VoxelityTabsEditorWindow>();
            window.Repaint();
        }
    }
}
