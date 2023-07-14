using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Voxelity.Editor.Tabs;
using Voxelity.Editor;

namespace Voxelity.AddOns.Editor
{
    public class AddOnsTab : Tab
    {
        private TabData setting = new TabData("Add-Ons", -1001, "Voxelity Add-Ons", EditorGUIUtility.IconContent("d_CustomTool").image);
        bool[] isInstalled;
        string[] addonFolders;
        string[] addonFoldersInProject;
        AddOnInfo[] addons;

        public override void OnSelected()
        {
            Refresh();
        }
        private void Refresh()
        {
            addonFolders = AddOnsUtility.GetAvailableFolderNames();
            addonFoldersInProject = AddOnsUtility.GetImportedFolderNames();
            addons = AddOnsUtility.GetAvaliablePacks();
            isInstalled = new bool[addons.Length];
            for (int i = 0; i < isInstalled.Length; i++)
            {
                isInstalled[i] = HasMatch(addonFoldersInProject, addonFolders[i]);
            }
        }
        private bool HasMatch(string[] values, string value)
        {
            if(values == null || values.Length == 0)
            {
                return false;
            }
            if (values.Contains(value)) return true;
            return false;
        }
        public override void OnGUI()
        {
            for (int i = 0; i < addons.Length; i++)
            {
                DisplayAddOn(addons[i], isInstalled[i]);
            }
        }
        private void DisplayAddOn(AddOnInfo addOn, bool isPackInstalled)
        {
            VoxelityGUI.DisplayInBox(() =>
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(addOn.packName, VoxelityGUI.BoldLabel(TextAnchor.UpperLeft), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField((isPackInstalled ? "√" : "〤"), GUILayout.Width(12), GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                VoxelityGUI.Line();
                if (addOn.description != "")
                    EditorGUILayout.LabelField(addOn.description,new GUIStyle(GUI.skin.label){ richText=true ,wordWrap = true});

                EditorGUILayout.LabelField("Version : " + addOn.version.ToVersionString());
                if (!isPackInstalled)
                {
                    if (VoxelityGUI.Button("Install", GUILayout.Width(50), GUILayout.Height(17)))
                    {
                        AddOnsUtility.ImportPackage(addOn);
                    }
                }
                else
                {
                    bool hasUpdate = false;
                    string addOnInProjectPath = Path.Combine(AddOnsUtility.AddOnsFullPathImported, addOn.packName);
                    if (!File.Exists(AddOnsUtility.GetDataInfoPath(addOn.packName, Path.Combine(AddOnsUtility.AddOnsFullPathImported, addOn.packName))))
                    {
                        hasUpdate = true;
                        EditorGUILayout.LabelField("Version info not available");
                    }
                    else
                    {
                        AddOnInfo projectInfo = AddOnsUtility.GetAddOnInfoAt(addOn.packName, addOnInProjectPath);
                        if (projectInfo.version < addOn.version)
                        {
                            EditorGUILayout.LabelField("Current Version : " + projectInfo.version.ToVersionString());
                            hasUpdate = true;
                        }
                    }

                    string[] buttonNames = new string[2] { hasUpdate ? "Update" : "Re-Install", "Remove" };
                    if (VoxelityGUI.InLineButtons(() =>
                    {
                        EditorGUILayout.Space(10);
                    }, buttonNames, out int pressedIndex, true, GUILayout.Width(75), GUILayout.Height(17)))
                    {
                        AssetDatabase.DeleteAsset(Path.Combine(AddOnsUtility.AddOnsPathImported, addOn.packName));
                        AssetDatabase.Refresh();
                        if (pressedIndex == 0)
                        {
                            AddOnsUtility.ImportPackageSilent(addOn);
                        }
                        Refresh();
                        return;
                    }
                }
            });
        }

        public override TabData TabInfo() => setting;
    }
}
