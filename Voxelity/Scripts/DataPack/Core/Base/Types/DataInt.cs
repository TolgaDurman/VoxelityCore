namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataInt : Data<int>
    {
        public DataInt(DataInfo info, int value) : base(info, value)
        {
        }
    }
}
