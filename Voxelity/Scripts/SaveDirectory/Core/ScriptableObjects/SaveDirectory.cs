using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voxelity.Save
{
    [CreateAssetMenu(menuName = SavableInfo.c_AssetMenuHead + "Save Directory"), System.Serializable]
    public class SaveDirectory : ScriptableObject
    {
        public bool lockObj;
        public bool saveToProject;
        public string saveName = "";
        [SerializeField] private List<Savables> savables = new List<Savables>();

        private const string splitter = "\n,";
        public List<Savables> Savables
        {
            get => savables;
        }
        public void DeleteSaveFiles()
        {
            JsonSaver.Delete(saveName);
        }
        public Savables GetSavable(string name)
        {
            return Savables.Find(x => x.name == name);
        }
        public void Refresh()
        {
            foreach (var item in savables)
            {
                item.directory = this;
            }
        }

        private string GetSavablesToString()
        {
            string combinedJson = "";
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is SavableInt)
                {
                    string savedData = JsonUtility.ToJson(new SaveData<int>(savables[i].name, ((SavableInt)savables[i]).Value));
                    combinedJson += savedData + splitter;
                }
                else if (savables[i] is SavableString)
                {
                    string savedData = JsonUtility.ToJson(new SaveData<string>(savables[i].name, ((SavableString)savables[i]).Value));
                    combinedJson += savedData + splitter;
                }
                else if (savables[i] is SavableFloat)
                {
                    string savedData = JsonUtility.ToJson(new SaveData<float>(savables[i].name, ((SavableFloat)savables[i]).Value));
                    combinedJson += savedData + splitter;
                }
                else if (savables[i] is SavableBool)
                {
                    string savedData = JsonUtility.ToJson(new SaveData<bool>(savables[i].name, ((SavableBool)savables[i]).Value));
                    combinedJson += savedData + splitter;
                }
                else if (savables[i] is SavableVector3)
                {
                    string savedData = JsonUtility.ToJson(new SaveData<Vector3>(savables[i].name, ((SavableVector3)savables[i]).Value));
                    combinedJson += savedData + splitter;
                }
            }
            return combinedJson;
        }
        private void SetSavesFromString(string value)
        {
            List<string> breaked = value.Split(splitter).ToList();
            breaked.RemoveAt(breaked.Count - 1);
            for (int i = 0; i < breaked.Count; i++)
            {
                if (savables[i] is SavableInt)
                {
                    ((SavableInt)savables[i]).Value = JsonUtility.FromJson<SaveData<int>>(breaked[i]).Value;
                }
                else if (savables[i] is SavableString)
                {
                    ((SavableString)savables[i]).Value = JsonUtility.FromJson<SaveData<string>>(breaked[i]).Value;
                }
                else if (savables[i] is SavableFloat)
                {
                    ((SavableFloat)savables[i]).Value = JsonUtility.FromJson<SaveData<float>>(breaked[i]).Value;
                }
                else if (savables[i] is SavableBool)
                {
                    ((SavableBool)savables[i]).Value = JsonUtility.FromJson<SaveData<bool>>(breaked[i]).Value;
                }
                else if (savables[i] is SavableVector3)
                {
                    ((SavableVector3)savables[i]).Value = JsonUtility.FromJson<SaveData<Vector3>>(breaked[i]).Value;
                }
            }
        }

        public void Save()
        {
            JsonSaver.SaveRaw(saveName, GetSavablesToString(), saveToProject);
        }
        public void Load()
        {
            if (!JsonSaver.Exists(saveName,saveToProject))
            {
                Debug.Log("save created");
                Save();
            }
            SetSavesFromString(JsonSaver.LoadRaw(saveName, saveToProject));
        }

#if UNITY_EDITOR
        public void AddSavable<T>(SaveData<T> data)
        {
            EditorUtility.SetDirty(this);
            Savables addedObj = null;
            switch (data.Value)
            {
                case int i:
                    addedObj = ScriptableObject.CreateInstance<SavableInt>();
                    break;
                case string s:
                    addedObj = ScriptableObject.CreateInstance<SavableString>();
                    break;
                case float f:
                    addedObj = ScriptableObject.CreateInstance<SavableFloat>();
                    break;
                case bool b:
                    addedObj = ScriptableObject.CreateInstance<SavableBool>();
                    break;
                case Vector3 v:
                    addedObj = ScriptableObject.CreateInstance<SavableVector3>();
                    break;
                default:
                    throw new ArgumentException("Invalid value type");
            }
            ((Savable<T>)addedObj).Value = data.Value;
            addedObj.name = data.Name;
            ((Savable<T>)addedObj).directory = this;
            AssetDatabase.SaveAssetIfDirty(addedObj);
            savables.Add(addedObj);
            AssetDatabase.AddObjectToAsset(addedObj, this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
        public void RemoveSavable(Savables item)
        {
            if (savables.Contains(item))
            {
                savables.Remove(item);
                EditorUtility.SetDirty(this);
                AssetDatabase.RemoveObjectFromAsset(item);
                GameObject.DestroyImmediate(item);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();
                Save();
            }
        }
        public void RemoveAll()
        {
            EditorUtility.SetDirty(this);
            foreach (var item in savables)
            {
                AssetDatabase.RemoveObjectFromAsset(item);
                GameObject.DestroyImmediate(item);
            }
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
            savables.Clear();
            DeleteSaveFiles();
        }
#endif
    }
}