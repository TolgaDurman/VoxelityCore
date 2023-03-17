using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Voxelity.Editor
{
    public static class PackageJsonUtility
    {
        public static string PackageManifestPath = Application.dataPath.TrimEnd("Assets".ToCharArray()) + "Packages/co.voxelstudio.voxelity/package.json";

        public static SampleItem[] Samples
        {
            get
            {
                // load the JSON file
                JObject jsonObj = PackageJsonUtility.GetPackageJson();

                // find the index of the sample to delete
                JArray samplesArray = (JArray)jsonObj["samples"];

                return samplesArray.ToSampleItemArray();
            }
        }
        public static SampleItem FindSample(string name)
        {
            foreach (var item in Samples)
            {
                if(item.displayName == name)
                    return item;
            }
            return null;
        }
        public static JObject GetPackageJson()
        {
            string json = File.ReadAllText(PackageManifestPath);
            JObject jsonObj = JObject.Parse(json);
            return jsonObj;
        }
    }

    [System.Serializable]
    public class SampleItem
    {
        public string displayName;
        public string description;
        public string path;
        public bool interactiveImport;

        public SampleItem() { }
        public SampleItem(string displayName, string description, string path, bool interactiveImport)
        {
            this.displayName = displayName;
            this.description = description;
            this.path = path;
            this.interactiveImport = interactiveImport;
        }
        public SampleItem(JObject obj)
        {
            try
            {
                this.displayName = obj["displayName"].Value<string>();
                this.description = obj["description"].Value<string>();
                this.path = obj["path"].Value<string>();
                this.interactiveImport = obj["interactiveImport"].Value<bool>();
            }
            catch (System.Exception)
            {
                throw new VoxelityException("Can't convert JObject to SampleItem");
            }
        }
        public JObject ToJObject()
        {
            JObject newSample = new JObject();
            newSample["displayName"] = displayName;
            newSample["description"] = description;
            newSample["path"] = path;
            newSample["interactiveImport"] = interactiveImport;
            return newSample;
        }
    }
    public static class SamplesUtility
    {
        public static SampleItem DisplaySample(SampleItem item)
        {
            EditorGUILayout.BeginVertical("box");
            item.displayName = EditorGUILayout.TextField("Display Name", item.displayName);
            item.description = EditorGUILayout.TextField("Description", item.description);
            item.path = EditorGUILayout.TextField("Path", item.path);
            item.interactiveImport = EditorGUILayout.Toggle("Interactive Import", item.interactiveImport);
            EditorGUILayout.EndVertical();
            return item;
        }
        public static void ToSampleItemArrayWithRef(this JArray array, ref SampleItem[] sampleItems)
        {
            ArrayUtility.Clear(ref sampleItems);
            foreach (var item in array)
            {
                ArrayUtility.Add(ref sampleItems, new SampleItem((JObject)item));
            }
        }
        public static SampleItem[] ToSampleItemArray(this JArray array)
        {
            SampleItem[] sampleItems = new SampleItem[0];
            foreach (var item in array)
            {
                ArrayUtility.Add(ref sampleItems, new SampleItem((JObject)item));
            }
            return sampleItems;
        }
        
        public static void ToJArray(this SampleItem[] array, ref JArray jArray)
        {
            jArray.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                jArray.Add(array[i].ToJObject());
            }
        }
        public static void ToJArray(this List<SampleItem> array, ref JArray jArray)
        {
            jArray.Clear();
            for (int i = 0; i < array.Count; i++)
            {
                jArray.Add(array[i].ToJObject());
            }
        }
    }
}
