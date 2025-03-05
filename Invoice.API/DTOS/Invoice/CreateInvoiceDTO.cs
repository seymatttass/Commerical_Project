namespace Invoice.API.DTOS.Invoice
{
    public class CreateInvoiceDTO
    {
        public int OrderId { get; set; }
        public int Cargoficheno { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
