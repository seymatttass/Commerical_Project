using Basket.API.Data.Entities;

namespace Basket.API.Data.ViewModels
{
    public class AddToBasketVM
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }


        public List<BasketItemVM> BasketItems { get; set; }

    }
}