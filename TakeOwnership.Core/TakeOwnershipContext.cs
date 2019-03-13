using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TakeOwnership
{
    using Interfaces;
    public class TakeOwnershipContext
    {
        /// <summary>
        /// Command line options
        /// </summary>
        public IOption Options { get; set; }

        /// <summary>
        /// New object owner account
        /// </summary>
        public NTAccount Owner { get; set; }
    }
}
