using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableString : SavableBase<string>
    {
        [SerializeField]private SaveData<string> saveData;
        internal override SaveData<string> Data 
        { 
            get => saveData; 
            set => saveData = value; 
        }
        public override string SetValue { set { saveData.Value = value; Save(); } }
    }
}
