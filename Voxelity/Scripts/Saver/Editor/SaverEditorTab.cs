using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxelity.Editor;
using Voxelity.Editor.Tabs;
using Voxelity.Extensions.Utility;
using System.IO;
using VFileAccess = Voxelity.Saver.Core.Storage.FileAccess;
using Voxelity.Extensions;
using UnityEditor;

namespace Voxelity.Saver.Editor
{
    public class SaverEditorTab : VoxelityTab
    {
        VoxelityTabSetting _settings = new VoxelityTabSetting("Save");
        public override void OnGUI()
        {
            if (VoxelityGUI.Button("Open game saves path", GUILayout.Height(30)))
                OpenDirectory.OpenPersistentDataPath();
            if (VoxelityGUI.Button("Delete All Saves", GUILayout.Height(30)))
                EditorHelper.DeleteVisibleFilesAndFolders(VFileAccess.BasePath);
            DisplaySaveWriters();
            DisplaySaveReaders();
            DisplayCurrentSaves();
        }
        private void DisplaySaveWriters()
        {
            VoxelityGUI.DisplayInBox(() =>
            {
                foreach (var item in VoxelitySaver.WritersCached)
                {
                    EditorGUILayout.BeginHorizontal();
                    VoxelityGUI.Label(item);
                    EditorGUILayout.EndHorizontal();
                }
            }, "Writers");
        }
        private void DisplaySaveReaders()
        {
            VoxelityGUI.DisplayInBox(() =>
            {
                foreach (var item in VoxelitySaver.ReadersCached)
                {
                    EditorGUILayout.BeginHorizontal();
                    VoxelityGUI.Label(item);
                    EditorGUILayout.EndHorizontal();
                }
            }, "Readers");
        }

        private void DisplayCurrentSaves()
        {
            string[] files = Directory.GetFiles(VFileAccess.BasePath, "*.json");
            VoxelityGUI.DisplayInBox(() =>
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(files[i]);
                    if (files[i] == "") continue;
                    VoxelityGUI.DisplayInBox(() =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        VoxelityGUI.Label(name.Colorize(Color.green), VoxelityGUI.TextStyle());
                        if (VoxelityGUI.Button("Open", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            EditorUtility.RevealInFinder(files[i]);
                        }
                        if (VoxelityGUI.Button("Delete", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            File.Delete(files[i]);
                        }
                        EditorGUILayout.EndHorizontal();
                    });
                }
            }, "Saved Jsons");
        }

        public override VoxelityTabSetting TabSettings()
        {
            return _settings;
        }
    }
}
