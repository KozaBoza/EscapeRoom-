using System;
using System.ComponentModel.DataAnnotations;
//obsługa płatności
namespace EscapeRoom.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Canceled
    }

    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        OnlinePayment
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być większa od zera.")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public PaymentMethod Method { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string TransactionId { get; set; }

        public string Notes { get; set; }
    }
}
