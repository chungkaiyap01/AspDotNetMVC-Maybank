using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maybank.DomainModelEntity.Enums
{
    public enum TransferMode
    {
        IBG = 0,

        IBGT = 1,

        [Display(Name = "Third Party Transfer")]
        Third_Party_Transfer = 2
    }
}
