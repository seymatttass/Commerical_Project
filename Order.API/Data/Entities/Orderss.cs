using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Order.API.Data.Enums;

namespace Order.API.Data.Entities
{
    public class Orderss    
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; } 
        public int BasketId { get; set; } 
        public int OrderItemId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrdeStatus OrderStatus { get; set; }
        public DateTime CretaedDate { get; set; }   
    }
}
