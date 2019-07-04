using Maybank.DomainModelEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maybank.DomainModelEntity.Entities
{
    public partial class BankTransaction
    {
        [Key]
        [Display(Name = "Bank Transaction ID")]
        public int ID { get; set; }

        [Display(Name = "Bank Account ID")]
        [ForeignKey("BankAccount")]
        public int BankAccountID { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        [Display(Name = "Transaction Date Time")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDateTime { get; set; }

        [Required(ErrorMessage = "Please enter your transaction amount.")]
        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "Please select your transaction amount.")]
        [Display(Name = "Tansfer Option")]
        public TransferMode TransferMode { get; set; }

        public string Remarks { get; set; }
    }
}
