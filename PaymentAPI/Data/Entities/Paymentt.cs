namespace Payment.API.Data.Entities
{
    public class Paymentt
    {
        public int ID { get; set; } 
        public int OrderId { get; set; }
        public int BasketId { get; set; } 
        public int PaymentType { get; set; }
        public DateTime Date { get; set; }
        public float PaymentTotal { get; set; } 
    }
}
