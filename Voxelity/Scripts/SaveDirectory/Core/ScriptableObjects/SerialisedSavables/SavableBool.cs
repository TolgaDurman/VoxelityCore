using UnityEngine;

namespace Voxelity.Save
{
    public class SavableBool : SavableBase<bool>
    {
        [SerializeField]private SaveData<bool> saveData;
        internal override SaveData<bool> Data
        {
            get => saveData;
            set => saveData = value;
        }
        public override bool SetValue { set { saveData.Value = value; Save(); } }
    }
}
