using UnityEngine;

namespace Voxelity.DataPacks.SaveDir
{
    [System.Serializable]
    public class SavableInt : Savable<int>
    {
        [SerializeField] private int value;
        public override int Value
        {
            get => value; set => this.value = value; 
        }
    }
}
