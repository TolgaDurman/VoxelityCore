using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Voxelity.Editor.Tabs;
using Voxelity.Extensions;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Voxelity.Editor
{
    public class VoxelityPacksTab : VoxelityTab
    {
        private VoxelityTabSetting setting = new("Add-Ons", -1, "Voxelity Add-Ons");
        private string packsPath = Application.dataPath.TrimEnd("Assets".ToCharArray()) + "Packages/co.voxelstudio.voxelity/Samples~";
        private string addOnsPath = Application.dataPath + "/Voxel Studio/Add-Ons";
        private string addOnsPathInAssets = "Assets/Voxel Studio/Add-Ons";
        private string[] folders;
        private string[] foldersInProject;
        public override void OnSelected()
        {
            folders = Directory.GetDirectories(packsPath);
            foldersInProject = Directory.GetDirectories(addOnsPath);
        }
        private void ImportPackage(string name,bool interactiveImport)
        {
            string packagePath = Directory.GetFiles(folders[IndexInProjectFolders(name)])[0];
            string path = packagePath.Substring(packagePath.IndexOf("Packages"));
            AssetDatabase.ImportPackage(path, interactiveImport);
        }
        public override void OnGUI()
        {
            for (int i = 0; i < folders.Length; i++)
            {
                string folderName = Path.GetFileName(folders[i]);
                VoxelityGUI.DisplayInBox(() =>
                {
                    EditorGUILayout.LabelField(folderName, VoxelityGUI.BoldLabel(TextAnchor.UpperLeft));
                    VoxelityGUI.Line();
                    SampleItem sample = PackageJsonUtility.FindSample(folderName);
                    if (sample.description != "") EditorGUILayout.LabelField("Version : " + sample.description);
                    bool isInstalled = IsPackInstalled(folderName);
                    EditorGUILayout.LabelField("Installed : " + (isInstalled ? "√" : "〤"));
                    if (!isInstalled)
                    {
                        if (VoxelityGUI.Button("Install", GUILayout.Width(50), GUILayout.Height(17)))
                        {
                            ImportPackage(folderName, sample.interactiveImport);
                        }
                    }
                    else
                    {
                        if (VoxelityGUI.InLineButtons(() =>
                        {
                            EditorGUILayout.Space(10);
                        }, new string[2] { "Re-Install", "Remove" }, out int pressedIndex, true, GUILayout.Width(75), GUILayout.Height(17)))
                        {
                            AssetDatabase.DeleteAsset(Path.Combine(addOnsPathInAssets, folderName));
                            AssetDatabase.Refresh();
                            if (pressedIndex == 0)
                            {
                                ImportPackage(folderName, false);
                            }
                        }
                    }
                });
            }
        }
        private int IndexInProjectFolders(string name)
        {
            for (int i = 0; i < folders.Length; i++)
            {
                if (Path.GetFileName(folders[i]) == name) return i;
            }
            return -1;
        }
        private bool IsPackInstalled(string packName)
        {
            for (int i = 0; i < foldersInProject.Length; i++)
            {
                if (Path.GetFileName(foldersInProject[i]) != packName) continue;
                return true;
            }
            return false;
        }

        public override VoxelityTabSetting TabSettings() => setting;
    }
}
