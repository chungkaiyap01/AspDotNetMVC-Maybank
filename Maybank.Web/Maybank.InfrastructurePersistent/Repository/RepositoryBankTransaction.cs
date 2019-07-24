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
    public class RepositoryBankTransaction : RepositoryGeneric<BankTransaction>, IRepositoryBankTransaction
    {

        public RepositoryBankTransaction(AppDbContext db) : base(db)
        {

        }

        public IEnumerable<BankTransaction> ReadByBankAccountID(int BankAccountID)
        {
            return db.BankTransaction.Where(x => x.BankAccountID == BankAccountID).ToList();
        }
    }
}
