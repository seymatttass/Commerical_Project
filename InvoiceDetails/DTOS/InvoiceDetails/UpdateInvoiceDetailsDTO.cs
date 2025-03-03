using InvoiceDetails.Mapping;

namespace InvoiceDetails.DTOS.InvoiceDetails
{
    public class UpdateInvoiceDetailsDTO : InvoiceDetailsAutoMapperProfile
    {
        public int InvoiceDetailsId { get; set; }
        public int InvoiceId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
    }
}
