namespace Voxelity.Save
{
    public class SavableBool : SavableBase<bool>
    {
        public SaveData<bool> saveData;
        public override SaveData<bool> Data 
        { 
            get => saveData; 
            set => saveData = value; 
        }
    }
}
