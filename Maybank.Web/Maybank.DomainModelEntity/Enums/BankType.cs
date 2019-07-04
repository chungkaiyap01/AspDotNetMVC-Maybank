using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maybank.DomainModelEntity.Enums
{
    public enum BankType
    {
        Maybank = 0,

        [Display(Name = "Public Bank")]
        Public_Bank = 1,

        [Display(Name = "CIMB Bank")]
        CIMB_Bank = 2,
    }
}
