namespace Basket.API.DTOS.BasketDTO
{
    public class AddToBasketItemDTO
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
