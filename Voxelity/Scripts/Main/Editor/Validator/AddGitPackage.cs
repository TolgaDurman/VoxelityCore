using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Voxelity.Editor
{
    public class AddGitPackage : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = GetWindow<AddGitPackage>();
            window.titleContent = new GUIContent("Add Git Package");
        }

        private void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };
            labelStyle.normal.textColor = Color.white;
            GUILayout.Label("Fix missing packages", labelStyle);
            var gitUrl = EditorGUILayout.TextField("Missing Pack Git URL:", VoxelityValidator.VoxelityPackageGitURL);
            if (GUILayout.Button("Add Package"))
            {
                AddPackage(gitUrl);
            }
        }

        public static void AddPackage(string gitUrl)
        {
            var addRequest = Client.Add(gitUrl);
            while (!addRequest.IsCompleted)
            {
                EditorUtility.DisplayProgressBar("Adding Package", "Adding package from Git URL...", ((float)addRequest.Status));
            }
            EditorUtility.ClearProgressBar();

            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Package added successfully.");
            }
            else
            {
                Debug.LogError("Failed to add package: " + addRequest.Error.message);
            }
        }
    }
}
