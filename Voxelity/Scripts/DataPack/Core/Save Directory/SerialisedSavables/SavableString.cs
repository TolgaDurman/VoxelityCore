using UnityEngine;

namespace Voxelity.DataPacks.SaveDir
{
    [System.Serializable]
    public class SavableString : Savable<string>
    {
        [SerializeField] private string value;
        public override string Value
        {
            get => value; set => this.value = value;
        }
    }
}
