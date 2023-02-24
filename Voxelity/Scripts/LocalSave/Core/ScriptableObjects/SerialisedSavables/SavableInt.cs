using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableInt : SavableBase<int>
    {
        [SerializeField]private SaveData<int> saveData;
        internal override SaveData<int> Data
        { 
            get => saveData; 
            set => saveData = value; 
        }
        public override int SetValue { set { saveData.Value = value; Save(); } }
    }
}
