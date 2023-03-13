using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Voxelity.DataPacks
{
    public class DataPackConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataPack);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);
            return new DataPack()
            {
                _int = val["_int"].ToObject<List<DataInt>>(),
                _float = val["_float"].ToObject<List<DataFloat>>(),
                _string = val["_string"].ToObject<List<DataString>>(),
                _bool = val["_bool"].ToObject<List<DataBool>>(),
                _vector3 = val["_vector3"].ToObject<List<DataVector3>>(),
            };
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (DataPack)value;
            writer.WriteStartObject();
            writer.WritePropertyName("_int");
            serializer.Serialize(writer, val._int);
            writer.WritePropertyName("_float");
            serializer.Serialize(writer, val._float);
            writer.WritePropertyName("_string");
            serializer.Serialize(writer, val._string);
            writer.WritePropertyName("_bool");
            serializer.Serialize(writer, val._bool);
            writer.WritePropertyName("_vector3");
            serializer.Serialize(writer, val._vector3);
            writer.WriteEndObject();
        }
    }
}
