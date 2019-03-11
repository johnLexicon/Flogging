using System;
using System.Collections.Generic;

namespace Flogging.Core
{
    public class FlogDetail
    {
        public FlogDetail()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public string Message { get; set; }

        /*** WHERE properties ***/

        public string Product { get; set; }

        public string Layer { get; set; }

        public string Location { get; set; }

        public string HostName { get; set; }

        /*** WHO properties ***/

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        /*** EVERYTHING ELSE ***/

        public long? ElapsedMilliseconds { get; set; } // Only for performance entries

        public Exception Exception { get; set; } // The exception for error logging

        public string CorrelationId { get; set; } // exception shielding from server to client

        public Dictionary<string, object> AdditionalInfo { get; set; } // catch-all for anything else



    }
}
