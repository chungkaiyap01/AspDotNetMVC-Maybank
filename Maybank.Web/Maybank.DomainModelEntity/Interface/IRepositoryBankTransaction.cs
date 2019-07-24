using System;
using System.Collections.Generic;
using System.Text;
using Maybank.DomainModelEntity.Entities;

namespace Maybank.DomainModelEntity.Interface
{
    public interface IRepositoryBankTransaction : IRepositoryGeneric<BankTransaction>
    {
        IEnumerable<BankTransaction> ReadByBankAccountID(int BankAccountID);
    }
}
