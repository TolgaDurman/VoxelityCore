using UnityEngine;

namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableVector3 : SavableBase<Vector3>
    {
        public SaveData<Vector3> saveData;
        public override SaveData<Vector3> Data 
        { 
            get => saveData; 
            set => saveData = value;
        }
    }
}
