using Address.API.Data;
using MassTransit;
using Shared.Events.AddressEvent;
using System.Collections.Concurrent;

namespace Address.API.Consumers
{
    public class GetAddressDetailRequestConsumer(AddressDbContext addressDbContext) : IConsumer<GetAddressDetailRequestEvent>
    {

        public async Task Consume(ConsumeContext<GetAddressDetailRequestEvent> context)
        {
            var addressId = context.Message.AddressId;
            var address = await addressDbContext.Addres.FindAsync(addressId);

            if (address != null)
            {
                var response = new GetAddressDetailResponseEvent(context.Message.CorrelationId)
                {
                    AddressId = address.Id,
                    Title = address.Title,
                    Country = address.Country,
                    City = address.City,
                    District = address.District,
                    PostalCode = address.PostalCode,
                    AddressText = address.AddressText,
                };

                await context.RespondAsync(response);
            }
            else
            {
                await context.RespondAsync(new
                {
                    Message = "Adres bulunamadı",
                    CorrelationId = context.Message.CorrelationId
                });
            }
        }
    }
}

