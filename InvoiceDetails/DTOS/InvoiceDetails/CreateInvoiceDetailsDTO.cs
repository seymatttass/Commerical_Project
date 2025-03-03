using InvoiceDetails.Mapping;

namespace InvoiceDetails.DTOS.InvoiceDetails
{
    public class CreateInvoiceDetailsDTO : InvoiceDetailsAutoMapperProfile
    {
        public int InvoiceId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
    }
}
