namespace Voxelity.Save
{
    [System.Serializable]
    public class SavableString : SavableBase<string>
    {
        public SaveData<string> saveData;
        public override SaveData<string> Data 
        { 
            get => saveData; 
            set => saveData = value; 
        }
    }
}
