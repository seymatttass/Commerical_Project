using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Shipping.API.Data.Entities
{
    public class Shipping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int CargoCompanyName { get; set; }
        public bool Active { get; set; }
        public int Shipcharge { get; set; }
        public bool free { get; set; }
        public int EstimatedDays { get; set; }
    }
}
