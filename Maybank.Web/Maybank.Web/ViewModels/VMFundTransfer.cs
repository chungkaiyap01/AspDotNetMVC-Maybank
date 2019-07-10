using Maybank.DomainModelEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Maybank.Web.ViewModels
{
    public class VMFundTransfer
    {
        [Display(Name = "Account No.")]
        public long AccountNo { get; set; }

        [Required(ErrorMessage = "Please select your Account Type.")]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Display(Name = "Account Balance")]
        public decimal AccountBalance { get; set; }

        public long RecipientAccount { get; set; }

        [Required(ErrorMessage = "Please enter your transaction amount.")]
        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "Please select your transaction amount.")]
        [Display(Name = "Tansfer Option")]
        public TransferMode TransferMode { get; set; }
    }
}