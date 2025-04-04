// Invoice.API/Consumers/CreateInvoiceCommandConsumer.cs
using MassTransit;
using Shared.Events.InvoiceEvents;
using System;
using System.Threading.Tasks;
using Invoice.API.Data;
using Invoice.API.Data.Entities;

namespace Invoice.API.Consumers
{
    public class CreateInvoiceCommandConsumer(InvoiceDbContext invoiceDbContext) : IConsumer<CreateInvoiceCommandEvent>
    {
        public async Task Consume(ConsumeContext<CreateInvoiceCommandEvent> context)
        {
            try
            {
                var newInvoice = new Data.Entities.Invoice
                {
                    OrderId = context.Message.OrderId,
                    Date = DateTime.Now,
                    TotalPrice = context.Message.TotalPrice,
                    Cargoficheno = context.Message.Cargoficheno
                };

                await invoiceDbContext.Invoices.AddAsync(newInvoice);
                await invoiceDbContext.SaveChangesAsync();

                var response = new InvoiceCreatedEvent(context.Message.CorrelationId)
                {
                    InvoiceId = newInvoice.Id,
                    OrderId = newInvoice.OrderId,
                    Date = newInvoice.Date,
                    TotalPrice = newInvoice.TotalPrice
                };

                await context.RespondAsync(response);
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new
                {
                    Message = $"Fatura oluşturulurken hata: {ex.Message}",
                    CorrelationId = context.Message.CorrelationId
                });
            }
        }
    }
}