using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableInt : Savable<int>
    {
        [SerializeField] private int value;
        public override int Value { get => value; set => this.value = value; }
    }
}
