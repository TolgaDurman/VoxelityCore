namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataString : Data<string>
    {
        public DataString(DataInfo info, string value) : base(info, value)
        {
        }
    }
}

