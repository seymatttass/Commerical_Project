namespace Basket.API.Data.ViewModels
{
    public class BasketItemVM
    {
        public decimal Price { get; set; }
        public int Count { get; set; }
        public int BasketID { get; set; }
        public int ProductId { get; set; }
    }
}
