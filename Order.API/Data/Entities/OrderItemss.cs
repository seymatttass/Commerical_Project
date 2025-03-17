using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Data.Entities
{
    public class OrderItemss    
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProductId { get; set; }
        public int ShippingId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }
}