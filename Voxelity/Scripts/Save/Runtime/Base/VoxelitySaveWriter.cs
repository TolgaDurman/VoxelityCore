using Voxelity.Saver.Core.Serialisers;

namespace Voxelity.Saver
{
    public class VoxelitySaveWriter : VoxelitySaverBase
    {
        private VoxelitySaveWriter(string root, VoxelitySaveSettings settings)
        {
            _root = root;
            _settings = settings;
        }

        /// <summary>
        /// Creates a VoxelitySaveWriter on the specified root
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <returns>A VoxelitySaveWriter instance</returns>
        public static VoxelitySaveWriter Create(string root)
        {
            return Create(root, new VoxelitySaveSettings());
        }

        /// <summary>
        /// Creates a VoxelitySaveWriter on the specified root using the specified settings
        /// </summary>
        /// <param name="root">The root to write to</param>
        /// <param name="settings">Settings</param>
        /// <returns>A VoxelitySaveWriter instance</returns>
        public static VoxelitySaveWriter Create(string root, VoxelitySaveSettings settings)
        {
            VoxelitySaveWriter saveWriter = new VoxelitySaveWriter(root, settings);
            saveWriter.Load(true);
            return saveWriter;
        }

        /// <summary>
        /// Writes an object to the specified key - you must called commit to write the data to file
        /// </summary>
        /// <typeparam name="T">The type of object to write</typeparam>
        /// <param name="key">The key this object will be saved under</param>
        /// <param name="value">The object to save</param>
        /// <returns>The VoxelitySaveWriter</returns>
        public VoxelitySaveWriter Write<T>(string key, T value)
        {
            if (Exists(key))
            {
                _items.Remove(key);
            }

            _items.Add(key, JsonSerialiser.SerialiseKey(value));

            return this;
        }

        /// <summary>
        /// Deletes the specified key if it exists
        /// </summary>
        /// <param name="key">The key to delete</param>
        public void Delete(string key)
        {
            if (Exists(key))
            {
                _items.Remove(key);
            }
        }

        /// <summary>
        /// Commits the changes to file
        /// </summary>
        public void Commit()
        {
            Save();
        }

        /// <summary>
        /// Attempts to commit the changes to file
        /// </summary>
        /// <returns>Was the commit successful</returns>
        public bool TryCommit()
        {
            try
            {
                Save();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}