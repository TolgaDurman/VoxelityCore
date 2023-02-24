using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableFloat : SavableBase<float>
    {
        [SerializeField]private SaveData<float> saveData;
        internal override SaveData<float> Data 
        { 
            get => saveData; 
            set => saveData = value; 
        }
        public override float SetValue { set { saveData.Value = value; Save(); } }
    }
}
