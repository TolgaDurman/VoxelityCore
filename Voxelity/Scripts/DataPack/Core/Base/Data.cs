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


        public void Save()
        {
            
        }
        public void Load()
        {
            
        }
    }
}
