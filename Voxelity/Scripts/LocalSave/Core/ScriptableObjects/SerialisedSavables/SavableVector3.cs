using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableVector3 : SavableBase<Vector3>
    {
        [SerializeField]private SaveData<Vector3> saveData;
        internal override SaveData<Vector3> Data 
        { 
            get => saveData; 
            set => saveData = value;
        }
        public override Vector3 SetValue { set { saveData.Value = value; Save(); } }
    }
}
