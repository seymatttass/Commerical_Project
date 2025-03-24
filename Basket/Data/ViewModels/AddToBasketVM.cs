namespace Basket.API.Data.ViewModels
{
    public class AddToBasketVM
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}