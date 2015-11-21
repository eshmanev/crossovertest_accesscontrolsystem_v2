using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync.Impl
{
    /// <summary>
    ///     Represents a serializable decorator of the <see cref="IChangeDataRetriever" />.
    /// </summary>
    [Serializable]
    internal class ChangeDataRetrieverDecorator : IChangeDataRetriever
    {
        private readonly Dictionary<Guid, object> _items = new Dictionary<Guid, object>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeDataRetrieverDecorator" /> class.
        /// </summary>
        /// <param name="changeDataRetriever">The change data retriever.</param>
        /// <param name="changeBatch">The change batch.</param>
        public ChangeDataRetrieverDecorator(object changeDataRetriever, ChangeBatch changeBatch)
            : this((IChangeDataRetriever) changeDataRetriever, changeBatch)
        {
            Contract.Requires(changeDataRetriever is IChangeDataRetriever);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeDataRetrieverDecorator" /> class.
        /// </summary>
        /// <param name="changeDataRetriever">The change data retriever.</param>
        /// <param name="changeBatch">The change batch.</param>
        public ChangeDataRetrieverDecorator(IChangeDataRetriever changeDataRetriever, ChangeBatch changeBatch)
        {
            IdFormats = changeDataRetriever.IdFormats;

            foreach (var itemChange in changeBatch)
            {
                var item = changeDataRetriever.LoadChangeData(new UserLoadChangeContext(IdFormats, itemChange));
                _items[itemChange.ItemId.GetGuidId()] = item;
            }
        }

        /// <summary>
        ///     Gets the identifier formats.
        /// </summary>
        /// <value>
        ///     The identifier formats.
        /// </value>
        public SyncIdFormatGroup IdFormats { get; }

        /// <summary>
        ///     Loads the change data.
        /// </summary>
        /// <param name="loadChangeContext">The load change context.</param>
        /// <returns></returns>
        public object LoadChangeData(LoadChangeContext loadChangeContext)
        {
            return _items[loadChangeContext.ItemChange.ItemId.GetGuidId()];
        }
    }
}