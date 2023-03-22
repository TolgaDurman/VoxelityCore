using UnityEngine;
using Voxelity.Saver;

namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class Data<T>
    {
        public DataInfo info;
        public T Value;
        [SerializeField] private T DefaultValue;
        public Data(DataInfo info, T value)
        {
            this.info = info;
            Value = value;
        }
        private VoxelitySaveWriter Writer
        {
            get => VoxelitySaver.GetWriter(info.root);
        }
        private VoxelitySaveReader Reader
        {
            get => VoxelitySaver.GetReader(info.root);
        }


        public void Save()
        {
            Writer.Write(info.objectName, Value);
            Writer.TryCommit();
        }
        public void LoadDefaultValue()
        {
            Writer.Write(info.objectName, DefaultValue);
            Writer.TryCommit();
            Reader.Reload();
            Reader.TryRead<T>(info.objectName, out Value);
        }
        public void Load()
        {
            Reader.Reload();
            if (!Reader.Exists(info.objectName))
            {
                Save();
            }
<<<<<<< Updated upstream
            Reader.TryRead<T>(info.objectName,out Value);
=======
            Reader.Reload();
            Reader.TryRead<T>(info.objectName, out Value);
>>>>>>> Stashed changes
        }
    }
}
