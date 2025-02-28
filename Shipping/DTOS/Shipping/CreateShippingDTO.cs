namespace Shipping.API.DTOS.Shipping
{
    public class CreateShippingDTO
    {

        public int CargoCompanyName { get; set; }
        public bool Active { get; set; }
        public int Shipcharge { get; set; }
        public bool Free { get; set; }

    }
}
