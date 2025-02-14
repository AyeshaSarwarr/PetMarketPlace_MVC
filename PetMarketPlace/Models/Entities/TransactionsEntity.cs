using System.ComponentModel.DataAnnotations;

namespace WebProject.Models.Entities
{
    public class TransactionsEntity
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required]
        public string PetType { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        [Phone] // Optional: validate phone number format
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(0, double.MaxValue)] // Ensure payment amount is non-negative
        public decimal PaymentAmount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
    }
}
