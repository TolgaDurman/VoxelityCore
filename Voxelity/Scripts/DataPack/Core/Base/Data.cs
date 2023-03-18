using Voxelity.Saver;

namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class Data<T>
    {
        public DataInfo info;
        public T Value;
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
        public void Load()
        {
            Reader.Reload();
            if (!Reader.Exists(info.objectName))
            {
                Save();
            }
            Reader.TryRead<T>(info.objectName,out Value);
        }
    }
}
