using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace Voxelity.DataPacks
{
    [JsonConverter(typeof(DataPackConverter))]
    [System.Serializable]
    public class DataPack
    {
        public List<DataInt> _int = new List<DataInt>();
        public List<DataFloat> _float = new List<DataFloat>();
        public List<DataString> _string = new List<DataString>();
        public List<DataBool> _bool = new List<DataBool>();
        public List<DataVector3> _vector3 = new List<DataVector3>();

        public DataPack(){}
    }
}
