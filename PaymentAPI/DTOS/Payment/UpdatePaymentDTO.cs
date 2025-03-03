using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.DTOS.Payments
{
    public class UpdatePaymentDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int BasketId { get; set; }

        [Required]
        public int PaymentType { get; set; } 

        [Required]
        public DateTime Date { get; set; } // Ödeme tarihi

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "PaymentTotal must be greater than zero.")]
        public float PaymentTotal { get; set; } 
    }
}
