using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOS
{
    public class CreateBasketItemDTO
    {
        [Required]
        public int BasketId { get; set; }  // Hangi sepete ait olduğunu belirtir

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }  // Ürünün fiyatı

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Count must be at least 1.")]
        public int Count { get; set; }  // Sepetteki ürün adedi
    }
}
