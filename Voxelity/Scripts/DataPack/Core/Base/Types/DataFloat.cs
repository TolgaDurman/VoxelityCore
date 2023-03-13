namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataFloat : Data<float>
    {
        public DataFloat(DataInfo info, float value) : base(info, value)
        {
        }
    }
}

