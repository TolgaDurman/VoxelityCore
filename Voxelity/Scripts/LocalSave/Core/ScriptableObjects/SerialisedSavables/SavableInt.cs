namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableInt : SavableBase<int>
    {
        public SaveData<int> saveData;
        public override SaveData<int> Data
        { 
            get => saveData; 
            set => saveData = value; 
        }
    }
}
