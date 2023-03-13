using Voxelity.Saver;
namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class Data<T>
    {
        public DataInfo info;
        public T Value;
        private VoxelitySaveWriter writer;

        private VoxelitySaveWriter Writer
        {
            get
            {
                if (writer == null) 
                    writer = VoxelitySaveWriter.Create(info.root);
                return writer;
            }
        }
        private VoxelitySaveReader reader;

        private VoxelitySaveReader Reader
        {
            get
            {
                if (reader == null) 
                    reader = VoxelitySaveReader.Create(info.root);
                return reader;
            }
        }
        public Data(DataInfo info, T value)
        {
            this.info = info;
            Value = value;
        }


        public void Save()
        {
            Writer.Write(info.objectName, Value);
            Writer.Commit();
        }
        public void Load()
        {
            if (!VoxelitySaveRaw.Exists(info.root)&&!Reader.Exists(info.objectName))
                Save();
            
            reader.TryRead<T>(info.objectName,out Value);
        }
    }
}
