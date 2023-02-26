using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableString : Savable<string>
    {
        [SerializeField] private string value;
        public override string Value { get => value; set => this.value = value; }
    }
}
