using UnityEngine;
namespace Voxelity.DataPacks
{
    [System.Serializable]
    public class DataVector3 : Data<Vector3>
    {
        public DataVector3(DataInfo info, Vector3 value) : base(info, value)
        {
        }
    }
}

