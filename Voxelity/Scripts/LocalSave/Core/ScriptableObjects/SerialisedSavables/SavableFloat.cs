namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableFloat : SavableBase<float>
    {
        public SaveData<float> saveData;
        public override SaveData<float> Data 
        { 
            get => saveData; 
            set => saveData = value; 
        }
    }
}
