using Maybank.DomainModelEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maybank.DomainModelEntity.Entities
{
    public partial class BankAccount
    {
        [Key]
        [Display(Name = "Bank Account ID")]
        public int ID { get; set; }

        [Display(Name = "Customer ID")]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required(ErrorMessage = "Please select your Account Type.")]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Display(Name = "Account No.")]
        public long AccountNo { get; set; }

        [Display(Name = "Account Balance")]
        public decimal AccountBalance { get; set; }

        public BankType Bank { get; set; }

        public virtual ICollection<BankTransaction> BankTransaction { get; set; }
    }
}
