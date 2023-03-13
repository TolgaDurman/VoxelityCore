using Voxelity.Saver.Core.Serialisers;
using Newtonsoft.Json;
using UnityEngine;

namespace Voxelity.Saver
{
    public static class VoxelitySaverGlobalSettings
    {
        /// <summary>
        /// The path to save data to - defaults to Application.persistentDataPath
        /// </summary>
        public static string StorageLocation { get; set; } = Application.persistentDataPath;

        /// <summary>
        /// Register a new json converter
        /// </summary>
        /// <param name="converter">The json converter to register</param>
        public static void RegisterConverter(JsonConverter converter) => JsonSerialiser.RegisterConverter(converter);
    }
}