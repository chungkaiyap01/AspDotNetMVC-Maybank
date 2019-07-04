using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maybank.DomainModelEntity.Enums
{
    public enum AccountType
    {
        [Display(Name = "Savings Account")]
        Savings_Account = 0,

        [Display(Name = "Current Account")]
        Current_Account = 1,

        [Display(Name = "Fixed Deposit Account")]
        Fixed_Deposit_Account = 2
    }
}
