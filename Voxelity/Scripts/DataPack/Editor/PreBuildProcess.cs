using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Voxelity.DataPacks.Editor
{
    public class PreBuildScriptableObjectsReset : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            SavableObjectEditorUtility.ResetToDefaultValues();
            Debug.Log("ScriptableObjects reset before build");
        }
    }

}
