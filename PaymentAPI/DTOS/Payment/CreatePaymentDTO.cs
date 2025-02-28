using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.DTOS.Payments
{
    public class CreatePaymentDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int BasketId { get; set; }

        [Required]
        public int PaymentType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "PaymentTotal must be greater than zero.")]
        public float PaymentTotal { get; set; } 
    }
}
