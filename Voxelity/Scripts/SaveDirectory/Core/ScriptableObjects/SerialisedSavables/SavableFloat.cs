using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public sealed class SavableFloat : Savable<float>
    {
        [SerializeField] private float value;
        public override float Value { get => value; set => this.value = value; }
    }
}
