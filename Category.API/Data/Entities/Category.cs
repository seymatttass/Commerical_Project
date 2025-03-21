using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
//1
namespace Category.API.Data.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }


    }
}
