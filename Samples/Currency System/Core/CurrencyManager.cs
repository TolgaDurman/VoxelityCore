using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Voxelity.DataPacks;
using Voxelity.Saver;

namespace Voxelity.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        public string currencyName;
        private const string c_SaveName="_Save.vxl";

        [SerializeField]private Data<int> currency;

        public UnityEvent<int> OnCurrencyChange;
        public bool autoSave;
        public int Value
        {
            get
            {
                return currency.Value;
            }
            set
            {
                currency = new Data<int>(currency.info,value);
                OnCurrencyChange?.Invoke(value);
                if(autoSave) Save();
            }
        }

        private void OnEnable()
        {
            CurrencyUtility.AssignManager(this);
            Load();
        }
        private void OnDisable()
        {
            CurrencyUtility.RemoveManager(this);
        }
        public void Save()
        {
            // var writer = VoxelitySaveWriter.Create(currency.info.root, new VoxelitySaveSettings()
            // {
            //     Password = "keytocheat"
            // });
            // writer.Write(currency.info.objectName, Value);
            // writer.Commit();
        }
        public void Load()
        {
            // var reader = VoxelitySaveReader.Create(currency.info.root, new VoxelitySaveSettings()
            // {
            //     Password = "keytocheat"
            // });

        }
    }
}
