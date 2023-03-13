using UnityEngine;

namespace Voxelity.DataPacks.SaveDir
{
    [System.Serializable]
    public class SavableVector3 : Savable<Vector3>
    {
        [SerializeField] private Vector3 value;
        public override Vector3 Value
        {
            get => value; set => this.value = value;
        }
    }
}
