using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Shared.Models
{
    namespace Logging.Shared.Models
    {
        public class LogMessage
        {
            public string Service { get; set; }
            public string Level { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
            public string Exception { get; set; }
        }
    }
}
