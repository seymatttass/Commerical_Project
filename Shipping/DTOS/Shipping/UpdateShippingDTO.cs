namespace Shipping.API.DTOS.Shipping
{
    public class UpdateShippingDTO
    {
        public int ShippingId { get; set; }
        public int CargoCompanyName { get; set; }
        public bool Active { get; set; }
        public int Shipcharge { get; set; }
        public bool Free { get; set; }
        public int EstimatedDays { get; set; }
    }
}
