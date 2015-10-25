using System;

namespace AccessControl.Contracts.Impl
{
    public class GetCompanySites : IGetCompanySites
    {
        /// <summary>
        /// The requestId for eventing
        /// </summary>
        public Guid RequestId { get; protected set; }
    }
}