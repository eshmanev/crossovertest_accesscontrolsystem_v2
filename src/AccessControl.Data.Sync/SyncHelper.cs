using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AccessControl.Contracts.Commands.Synchronization;
using AccessControl.Data.Sync.Impl;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync
{
    public static class SyncHelper
    {
        public static ChangeBatch Read(this IGetChangeBatchResult result, out object changeDataRetriever)
        {
            var changeBatch = Deserialize<ChangeBatch>(result.ChangeBatch);
            changeDataRetriever = Deserialize<ChangeDataRetrieverDecorator>(result.ChangeDataRetriever);
            return changeBatch;
        }

        public static byte[] Serialize(ChangeBatch changeBatch)
        {
            return Serialize<ChangeBatch>(changeBatch);
        }

        public static byte[] Serialize(IChangeDataRetriever changeDataRetriever, ChangeBatch changeBatch)
        {
            var serializableChangeDataRetriever = new ChangeDataRetrieverDecorator(changeDataRetriever, changeBatch);
            return Serialize<ChangeDataRetrieverDecorator>(serializableChangeDataRetriever);
        }

        private static T Deserialize<T>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(stream);
            }
        }

        private static byte[] Serialize<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.GetBuffer();
            }
        }
    }
}