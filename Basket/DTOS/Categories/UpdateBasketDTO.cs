using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOS
{
    public class UpdateBasketDTO
    {
        [Required]
        public int Id { get; set; }  // Güncellenecek sepetin ID’si

        [Required]
        public int UserId { get; set; }  // Sepeti oluşturan kullanıcı

        [Required]
        public int StockId { get; set; }  // Güncellenecek ürünün stok ID’si

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }  // Güncellenmiş toplam fiyat
    }
}
