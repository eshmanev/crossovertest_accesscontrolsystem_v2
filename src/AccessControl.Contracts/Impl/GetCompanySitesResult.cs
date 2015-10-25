using System;

namespace AccessControl.Contracts.Impl
{
    public class GetCompanySitesResult : IGetCompanySitesResult
    {
        /// <summary>
        /// The requestId for eventing
        /// </summary>
        public Guid RequestId { get; protected set; }
    }
}