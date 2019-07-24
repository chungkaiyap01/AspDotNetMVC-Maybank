using System;
using System.Collections.Generic;
using System.Text;
using Maybank.DomainModelEntity.Entities;

namespace Maybank.DomainModelEntity.Interface
{
    public interface IRepositoryBankAccount : IRepositoryGeneric<BankAccount>
    {
        bool SearchDuplicateAccountNo(long AccountNo);

        BankAccount ReadByCustomerID(int CustomerID);

        IEnumerable<BankAccount> IReadByCustomerID(int CustomerID);

        BankAccount ReadByAccountNo(long AccountNo);
    }
}
