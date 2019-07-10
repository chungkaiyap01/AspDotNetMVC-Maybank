using Maybank.DomainModelEntity.Entities;
using Maybank.DomainModelEntity.Interface;
using Maybank.InfrastructurePersistent.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.InfrastructurePersistent.Repository
{
    public class RepositoryBankAccount : RepositoryGeneric<BankAccount>, IRepositoryBankAccount
    {
        public RepositoryBankAccount(AppDbContext db) : base(db)
        {

        }

        public BankAccount ReadByCustomerID(int CustomerID)
        {
            return db.BankAccount.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
        }

        public bool SearchDuplicateAccountNo(long AccountNo)
        {
            BankAccount bankAccount = db.BankAccount.Where(x => x.AccountNo == AccountNo).FirstOrDefault();

            if (bankAccount == null)
                return false;

            return true;
        }
    }
}
