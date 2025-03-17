using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.UserEvents
{
    public class GetUserDetailRequestEvent : CorrelatedBy<Guid>
    {
        // corrid: MassTransit'in istek-yanıt mekanizması için gereklidir. Bu ID, gönderilen isteği ve alınan yanıtı eşleştirmeye yarar

        public GetUserDetailRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }

        // İstek içinde ihtiyaç duyulan kullanıcı ID'si
        public int UserId { get; set; }

    }

}
