namespace Order.API.DTOS.OrdersDTO.Orders
{
    public class UpdateOrdersDTO
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
