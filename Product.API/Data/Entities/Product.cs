using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Data.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        public int Code { get; set; }
        public int Name { get; set; }
        public int Price { get; set; }
    }
}
