using System;
using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.Logs
{
    public class IndexViewModel
    {
        /// <summary>
        ///     Gets or sets From date.
        /// </summary>
        /// <value>
        ///     From date.
        /// </value>
        public string FromDate { get; set; }

        /// <summary>
        ///     Gets or sets the logs.
        /// </summary>
        /// <value>
        ///     The logs.
        /// </value>
        public ILogEntry[] Logs { get; set; }

        /// <summary>
        ///     Gets or sets To date.
        /// </summary>
        /// <value>
        ///     To date.
        /// </value>
        public string ToDate { get; set; }
    }
}