namespace Invoice.API.DTOS.Invoice
{
    public class UpdateInvoiceDTO
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public int Cargoficheno { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
