using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
using Voxelity.Extensions.Utility;

namespace Voxelity.Editor
{
    public static class VoxelityValidator
    {
        private const string VoxelityPackageId = "co.voxelstudio.voxelity";
        public const string VoxelityPackageGitURL = "https://github.com/TolgaDurman/VoxelityCore.git";
#if VOXELITY_CORE
        [MenuItem("Voxelity/Check Updates")]
        public static void GetVoxelityUpdate()
        {
            Debug.Log("Voxelity core available...");
            var packageRequest = Client.List(!CheckConnection.IsConnected());
            while (!packageRequest.IsCompleted)
            {
                // Display progress bar while waiting for the request to complete
                EditorUtility.DisplayProgressBar("Checking for updates", "Fetching package list...", ((float)packageRequest.Status));
            }
            EditorUtility.ClearProgressBar();

            if (packageRequest.Status == StatusCode.Success)
            {
                var packageInfo = packageRequest.Result.FirstOrDefault(p => p.name == VoxelityPackageId);
                if (packageInfo != null)
                {
                    var installedVersion = new Version(packageInfo.version);
                    var latestVersion = GetLatestPackageVersion();
                    if (installedVersion < latestVersion)
                    {
                        if (ShowUpdatePopupWindow(latestVersion.ToString()))
                        {
                            AddGitPackage.AddPackage(VoxelityPackageGitURL);
                        }
                    }
                    else
                    {
                        Debug.Log("Voxelity Up to date");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to list packages: " + packageRequest.Error.message);
            }
        }

        private static Version GetLatestPackageVersion()
        {
            // Use UnityWebRequest to fetch the package's latest version from GitHub API
            using (var www = UnityWebRequest.Get("https://raw.githubusercontent.com/TolgaDurman/VoxelityCore/main/package.json"))
            {
                www.SendWebRequest();

                while (!www.isDone)
                {
                    // Display progress bar while waiting for the request to complete
                    EditorUtility.DisplayProgressBar("Checking for updates", "Fetching latest version...", www.downloadProgress);
                }
                EditorUtility.ClearProgressBar();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // Parse the latest version from the response
                    var response = JsonUtility.FromJson<PackageInfo>(www.downloadHandler.text);
                    if (response != null && !string.IsNullOrEmpty(response.version))
                    {
                        return new Version(response.version);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch latest version: " + www.error);
                }
            }

            // Return version 0.0.0 if failed to fetch latest version
            return new Version(0, 0, 0);
        }

        private static bool ShowUpdatePopupWindow(string latestVersion)
        {
            return EditorUtility.DisplayDialog("Voxelity Core Update Available",
                $"A newer version of Voxelity Core ({latestVersion}) is available. Do you want to update it now?",
                "Yes", "No");
        }

        [System.Serializable]
        public class PackageInfo
        {
            public string name;
            public string version;
            public string displayName;
            public string description;
            public string documentationUrl;
            public string changelogUrl;
            public string licensesUrl;
            public string[] keywords;
            public string author;

        }
#endif
        public static bool IsPackageInstalled(string packageId)
        {
            if (!File.Exists("Packages/manifest.json"))
                return false;

            string jsonText = File.ReadAllText("Packages/manifest.json");
            return jsonText.Contains(packageId);
        }
    }
}
