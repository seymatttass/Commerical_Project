using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductShipping.Data.Entities
{
    public class ProductShipping
    {

        public int ProductId { get; set; }
        public int ShippingId { get; set; }
    }
}
