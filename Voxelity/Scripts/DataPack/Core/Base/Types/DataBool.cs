namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataBool : Data<bool>
    {
        public DataBool(DataInfo info, bool value) : base(info, value)
        {
        }
    }
}

