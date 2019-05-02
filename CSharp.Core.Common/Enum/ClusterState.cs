using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Cluster State
    /// </summary>
    public enum ClusterState
    {
        /// <summary>
        /// State Unknown
        /// </summary>
        StateUnknown=-1,
        /// <summary>
        /// Online
        /// </summary>
        Online = 0,
        /// <summary>
        /// Offline
        /// </summary>
        Offline = 1,
        /// <summary>
        /// Failed
        /// </summary>
        Failed = 2,
        /// <summary>
        /// PartialOnline
        /// </summary>
        PartialOnline = 3,
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 4,
        /// <summary>
        /// SystemCrash
        /// </summary>
        SystemCrash = 90
    }
}
