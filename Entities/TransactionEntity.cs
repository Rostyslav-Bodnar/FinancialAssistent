using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAssistent.Entities
{
    public class TransactionEntity
    {
        [Key]
        public string Id { get; set; } // Унікальний ID транзакції

        [ForeignKey("BankCard")]
        public int BankCardId { get; set; }
        public BankCardEntity BankCard { get; set; }

        public long Time { get; set; }
        public string Description { get; set; }
        public int Mcc { get; set; }
        public bool Hold { get; set; }
        public decimal Amount { get; set; }
        public decimal OperationAmount { get; set; }
        public int CurrencyCode { get; set; }
        public decimal CashbackAmount { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public string Comment { get; set; }
        public string ReceiptId { get; set; }
        public string InvoiceId { get; set; }
        public string CounterEdrpou { get; set; }
        public string CounterIban { get; set; }
        public string CounterName { get; set; }
    }
}
