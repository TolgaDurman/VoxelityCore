using UnityEngine;

namespace Voxelity.DataPacks.SaveDir
{
    [System.Serializable]
    public sealed class SavableBool : Savable<bool>
    {
        [SerializeField] private bool value;

        public override bool Value 
        { 
            get => value; set => this.value = value; 
         }
    }
}
