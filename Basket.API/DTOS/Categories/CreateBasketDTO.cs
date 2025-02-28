using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOS
{
    public class CreateBasketDTO
    {
        [Required]
        public int UserId { get; set; }   

        [Required]
        public int StockId { get; set; }  

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }  
    }
}
