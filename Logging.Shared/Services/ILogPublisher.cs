using Logging.Shared.Models;
using Logging.Shared.Models.Logging.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Shared.Services
{
    public interface ILogPublisher
    {
        Task PublishLogAsync(LogMessage message);
    }
}
