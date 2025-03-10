using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
    }
}
