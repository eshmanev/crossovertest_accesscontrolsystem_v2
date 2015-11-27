using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace AccessSimulator
{
    [Serializable]
    public class CacheData
    {
        private const string FileName = "cache.dat";

        public CacheItem[] AccessPoints { get; set; }
        public CacheItem[] Users { get; set; }

        public static void Save(CacheItem[] accessPoints, CacheItem[] users)
        {
            try
            {
                var cache = new CacheData
                {
                    AccessPoints = accessPoints,
                    Users = users
                };
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(FileName, FileMode.Create))
                {
                    formatter.Serialize(stream, cache);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void Load(out CacheItem[] accessPoints, out CacheItem[] users)
        {
            accessPoints  = new CacheItem[0];
            users = new CacheItem[0];
            if (!File.Exists(FileName))
                return;

            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(FileName, FileMode.Open))
                {
                    var deserialized = (CacheData)formatter.Deserialize(stream);
                    accessPoints = deserialized.AccessPoints;
                    users = deserialized.Users;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}