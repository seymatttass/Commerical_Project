using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.UserEvents
{
    public class GetUserDetailResponseEvent : CorrelatedBy<Guid>
    {
        public GetUserDetailResponseEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
