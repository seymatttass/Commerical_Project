using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDetails.Data.Entities
{
    public class InvoiceDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
    }
}
