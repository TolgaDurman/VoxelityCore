using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voxelity.Saver;
using Voxelity.Saver.Core.Storage;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voxelity.DataPacks.SaveDir
{
    [CreateAssetMenu(menuName = "Voxelity/Save Directory"), System.Serializable]
    public class SaveDirectory : ScriptableObject
    {
        public bool lockObj;
        [SerializeField] private List<Savables> savables = new List<Savables>();

        public VoxelitySaveWriter Writer
        {
            get
            {
                return VoxelitySaver.GetWriter(name);
            }
        }
        public VoxelitySaveReader Reader
        {
            get
            {
                return VoxelitySaver.GetReader(name);
            }
        }
        public List<Savables> Savables
        {
            get => savables;
        }
        public void DeleteSaveFile()
        {
            FileAccess.Delete(name, false);
            Debug.Log(name + " : Save File deleted");
        }
        public void Refresh()
        {
            foreach (var item in savables)
            {
                item.directory = this;
            }
        }
        public void Write<T>(Savable<T> savable)
        {
            Writer.Write(savable.name, savable.Value);
        }
        public void Commit()
        {
            Writer.Commit();
        }
        public void Load<T>(Savable<T> savable)
        {
            Reader.Reload();
            if (!Reader.Exists(savable.name))
            {
                Write<T>(savable);
                Commit();
            }
            Reader.TryRead<T>(savable.name, out T value);
            savable.Value = value;
        }

        public void SaveAll()
        {
            foreach (var item in savables)
            {
                item.Save();
            }
            Commit();
        }
        public void LoadAll()
        {
            foreach (var item in savables)
            {
                item.Load();
            }
        }

#if UNITY_EDITOR
        public void AddSavable<T>(Data<T> data)
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
            addedObj.name = data.info.objectName;
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
                SaveAll();
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
            DeleteSaveFile();
        }
#endif
    }
}