using Basket.API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOS.BasketDTO.Basket
{
    public class UpdateBasketDTO
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        public int UserId { get; set; } 

        [Required]
        public int StockId { get; set; }  

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; } 

        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
