using Basket.API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.DTOS.BasketDTO.Basket
{
    public class CreateBasketDTO
    {
        public int UserId { get; set; }
        public List<BasketItem> BasketItems { get; set; }

    }
}
