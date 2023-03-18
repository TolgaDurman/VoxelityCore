using System.Collections.Generic;
using System.Linq;

namespace Voxelity.Saver
{
    public static class VoxelitySaver
    {
        private static Dictionary<string,VoxelitySaveWriter> ActiveWriters = new Dictionary<string, VoxelitySaveWriter>();
        private static Dictionary<string,VoxelitySaveReader> ActiveReaders = new Dictionary<string, VoxelitySaveReader>();

        public static List<string> ReadersCached
        {
            get
            {
                return ActiveReaders.Keys.ToList();
            }
        }
        public static List<string> WritersCached
        {
            get
            {
                return ActiveWriters.Keys.ToList();
            }
        }
        
        public static VoxelitySaveWriter GetWriter(string root)
        {
            if(WriterExists(root))
                return ActiveWriters[root];
            
            var writer = VoxelitySaveWriter.Create(root);
            AssignWriter(root,writer);
            return writer;
        }
        public static VoxelitySaveReader GetReader(string root)
        {
            if(ReaderExists(root))
                return ActiveReaders[root];
            
            var reader = VoxelitySaveReader.SafeCreate(root);
            AssignReader(root,reader);
            return reader;
        }
        private static bool ReaderExists(string root)
        {
            if(ActiveReaders.ContainsKey(root)) return true;
            return false;
        }
        private static void AssignReader(string root,VoxelitySaveReader reader)
        {
            ActiveReaders.Add(root,reader);
        }
        

        private static bool WriterExists(string root)
        {
            if(ActiveWriters.ContainsKey(root)) return true;
            return false;
        }
        private static void AssignWriter(string root,VoxelitySaveWriter writer)
        {
            ActiveWriters.Add(root,writer);
        }
    }
}
