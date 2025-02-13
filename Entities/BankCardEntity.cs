using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAssistent.Entities
{
    public class BankCardEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Token { get; set; } // Токен для отримання даних з Monobank API

        public decimal Balance { get; set; } // Останній відомий баланс

        public List<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
    }
}
