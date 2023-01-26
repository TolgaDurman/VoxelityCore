using System;
using System.Collections;
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
        public string saveName = "SaveName.vxl";
        [SerializeField] private List<ScriptableObject> savables = new List<ScriptableObject>();

        private const string splitter = "\n~//~\n";
        public List<ScriptableObject> Savables
        {
            get => savables;
        }
        public void DeleteSaveFiles()
        {
            JsonSaver.Delete(saveName);
        }

        private string GetSavablesToString()
        {
            string combinedJson = "";
            foreach (var item in savables)
            {
                combinedJson += JsonUtility.ToJson(item) + splitter;
            }
            Debug.Log(combinedJson);
            return combinedJson;
        }
        private void SetSavesFromString(string value)
        {
            List<string> breaked = value.Split(splitter).ToList();
            breaked.RemoveAt(breaked.Count-1);
            for (int i = 0; i < breaked.Count; i++)
            {
                if(savables[i] is SavableInt)
                {
                    ((SavableInt)savables[i]).saveData = JsonUtility.FromJson<SaveData<int>>(breaked[i]);
                }
                else if(savables[i] is SavableString)
                {
                    ((SavableString)savables[i]).saveData = JsonUtility.FromJson<SaveData<string>>(breaked[i]);
                }
                else if(savables[i] is SavableFloat)
                {
                    ((SavableFloat)savables[i]).saveData = JsonUtility.FromJson<SaveData<float>>(breaked[i]);
                }
                else if(savables[i] is SavableBool)
                {
                    ((SavableBool)savables[i]).saveData = JsonUtility.FromJson<SaveData<bool>>(breaked[i]);
                }
                else if(savables[i] is SavableVector3)
                {
                    ((SavableVector3)savables[i]).saveData = JsonUtility.FromJson<SaveData<Vector3>>(breaked[i]);
                }
            }
        }

        public void SetSave(string valueName,int value)
        {
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is not SavableInt) continue;
                if (((SavableInt)savables[i]).Data.Name != valueName) continue;
                ((SavableInt)savables[i]).Data = new SaveData<int>(valueName, value);
                Save();
                return;
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }
        public void SetSave(string valueName, string value)
        {
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is not SavableString) continue;
                if (((SavableString)savables[i]).Data.Name != valueName) continue;
                ((SavableString)savables[i]).Data = new SaveData<string>(valueName, value);
                Save();
                return;
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }
        public void SetSave(string valueName, float value)
        {
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is not SavableFloat) continue;
                if (((SavableFloat)savables[i]).Data.Name != valueName) continue;
                ((SavableFloat)savables[i]).Data = new SaveData<float>(valueName, value);
                Save();
                return;
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }
        public void SetSave(string valueName, bool value)
        {
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is not SavableBool) continue;
                if (((SavableBool)savables[i]).Data.Name != valueName) continue;
                ((SavableBool)savables[i]).Data = new SaveData<bool>(valueName, value);
                Save();
                return;
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }
        public void SetSave(string valueName, Vector3 value)
        {
            for (int i = 0; i < savables.Count; i++)
            {
                if (savables[i] is not SavableVector3) continue;
                if (((SavableVector3)savables[i]).Data.Name != valueName) continue;
                ((SavableVector3)savables[i]).Data = new SaveData<Vector3>(valueName, value);
                Save();
                return;
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }

        public T GetValue<T>(string valueName)
        {
            foreach (var item in savables)
            {
                if(((SavableBase<T>)item).Data.Name == valueName)
                {
                    return ((SavableBase<T>)item).GetValue;
                }
            }
            throw new ArgumentException("Couldn't find the value in " + name + " named:" + valueName);
        }

        public void Save()
        {
            JsonSaver.SaveRaw(saveName, GetSavablesToString());
        }
        public void Load()
        {
            SetSavesFromString(JsonSaver.LoadRaw(saveName));
        }

#if UNITY_EDITOR
        public void AddSavable<T>(SaveData<T> data)
        {
            EditorUtility.SetDirty(this);
            ScriptableObject addedObj = null;
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
            ((SavableBase<T>)addedObj).Data = data;
            addedObj.name = data.Name;
            AssetDatabase.SaveAssetIfDirty(addedObj);
            savables.Add(addedObj);
            AssetDatabase.AddObjectToAsset(addedObj, this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
        public void RemoveSavable(ScriptableObject item)
        {
            if(savables.Contains(item))
            {
                savables.Remove(item);
                EditorUtility.SetDirty(this);
                AssetDatabase.RemoveObjectFromAsset(item);
                GameObject.DestroyImmediate(item);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();
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
        }
#endif
    }
}