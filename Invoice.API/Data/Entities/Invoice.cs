using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invoice.API.Data.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Cargoficheno { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
