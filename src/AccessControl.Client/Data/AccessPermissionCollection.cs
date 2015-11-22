using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class AccessPermissionCollection : IAccessPermissionCollection
    {
        private readonly ReaderWriterLockSlim _lockObj = new ReaderWriterLockSlim();
        private readonly HashSet<IAccessPermission> _permissions = new HashSet<IAccessPermission>();

        /// <summary>
        ///     Adds or updates the given permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void AddOrUpdatePermission(IAccessPermission permission)
        {
            _lockObj.EnterWriteLock();
            try
            {
                _permissions.RemoveWhere(x => Equals(x, permission));
                _permissions.Add(permission);
            }
            finally
            {
                _lockObj.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Removes the given permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void RemovePermission(IAccessPermission permission)
        {
            _lockObj.EnterWriteLock();
            try
            {
                _permissions.Remove(permission);
            }
            finally
            {
                _lockObj.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public bool IsAllowed(Guid accessPointId, string userHash)
        {
            if (accessPointId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(userHash))
                return false;

            _lockObj.EnterReadLock();
            try
            {
                return _permissions.Any(x => x.IsAllowed(accessPointId, userHash));
            }
            finally
            {
                _lockObj.ExitReadLock();
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IAccessPermission> GetEnumerator()
        {
            _lockObj.EnterReadLock();
            try
            {
                return new List<IAccessPermission>(_permissions).GetEnumerator();
            }
            finally
            {
                _lockObj.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}