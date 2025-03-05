namespace Order.API.DTOS.OrdersDTO.Orders
{
    public class CreateOrdersDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
