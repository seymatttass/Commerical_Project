using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Data.Entities
{
    public class BasketItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BasketId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
