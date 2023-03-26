using UnityEditor;

namespace Voxelity.Editor
{
    public class DefineSetter : UnityEditor.Build.IActiveBuildTargetChanged
    {
        private const string defineName = "VOXELITY_CORE";

        public int callbackOrder => -100;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            EditorHelper.AddSymbols(defineName);
        }
#if !VOXELITY_CORE
        [InitializeOnLoadMethod]
        private static void AddDefine()
        {
            EditorHelper.AddSymbols(defineName);
        }
#endif
    }
}
