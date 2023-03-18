using System;
using Voxelity.Saver.Core.Serialisers;

namespace Voxelity.Saver
{
    public class VoxelitySaveReader : VoxelitySaverBase
    {
        private VoxelitySaveReader(string root, VoxelitySaveSettings settings)
        {
            _root = root;
            _settings = settings;
        }

        /// <summary>
        /// Creates a VoxelitySaveReader on the specified root
        /// </summary>
        /// <param name="root">The root to read from</param>
        /// <returns>A VoxelitySaveReader instance</returns>
        public static VoxelitySaveReader Create(string root)
        {
            return Create(root, new VoxelitySaveSettings());
        }
         /// <summary>
        /// Safely Creates a VoxelitySaveReader on the specified root
        /// </summary>
        /// <param name="root">The root to read from</param>
        /// <returns>A VoxelitySaveReader instance</returns>
        public static VoxelitySaveReader SafeCreate(string root)
        {
            return Create(root, new VoxelitySaveSettings(), true);
        }

        /// <summary>
        /// Creates a VoxelitySaveReader on the specified root using the specified settings
        /// </summary>
        /// <param name="root">The root to read from</param>
        /// <param name="settings">Settings</param>
        /// <returns>A VoxelitySaveReader instance</returns>
        public static VoxelitySaveReader Create(string root, VoxelitySaveSettings settings, bool safeLoad = false)
        {
            VoxelitySaveReader saveReader = new VoxelitySaveReader(root, settings);
            saveReader.Load(safeLoad);
            return saveReader;
        }

        /// <summary>
        /// Reads an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <returns>The object that was loaded</returns>
        public T Read<T>(string key)
        {
            if (!Exists(key))
            {
                throw new VoxelityException("Key does not exists");
            }

            try
            {
                return JsonSerialiser.DeserialiseKey<T>(key, _items);
            }
            catch
            {
                throw new VoxelityException("Deserialisation failed");
            }
        }

        /// <summary>
        /// Reads an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="result">An action to be called when the read completes</param>
        /// <returns>The VoxelitySaveReader</returns>
        public VoxelitySaveReader Read<T>(string key, Action<T> result)
        {
            if (!Exists(key))
            {
                throw new VoxelityException("Key does not exists");
            }

            try
            {
                result(JsonSerialiser.DeserialiseKey<T>(key, _items));
            }
            catch
            {
                throw new VoxelityException("Deserialisation failed");
            }

            return this;
        }

        /// <summary>
        /// Attempts to read an object under the specified key
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <param name="key">The key this object was saved under</param>
        /// <param name="result">The object that was loaded</param>
        /// <returns>Was the read successful</returns>
        public bool TryRead<T>(string key, out T result)
        {
            result = default(T);

            if (!Exists(key))
            {
                return false;
            }

            try
            {
                result = JsonSerialiser.DeserialiseKey<T>(key, _items);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reloads data from the root
        /// </summary>
        public void Reload()
        {
            Load(true);
        }
    }
}