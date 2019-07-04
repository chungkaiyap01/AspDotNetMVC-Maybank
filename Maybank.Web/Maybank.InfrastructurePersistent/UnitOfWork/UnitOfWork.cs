using Maybank.DomainModelEntity.Interface;
using Maybank.InfrastructurePersistent.Context;
using Maybank.InfrastructurePersistent.Interface;
using Maybank.InfrastructurePersistent.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.InfrastructurePersistent.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext db;

        public UnitOfWork()
        {
            db = new AppDbContext();
        }
        
        private IRepositoryAdministrator _Administrator;
        public IRepositoryAdministrator Administrator
        {
            get
            {
                if(_Administrator == null)
                {
                    _Administrator = new RepositoryAdministrator(db);
                }
                return _Administrator;
            }
        }


        private IRepositoryBankAccount _BankAccount;
        public IRepositoryBankAccount BankAccount
        {
            get
            {
                if(_BankAccount == null)
                {
                    _BankAccount = new RepositoryBankAccount(db);
                }
                return _BankAccount;
            }
        }

        private IRepositoryBankTransaction _BankTransaction;
        public IRepositoryBankTransaction BankTransaction
        {
            get
            {
                if(_BankTransaction == null)
                {
                    _BankTransaction = new RepositoryBankTransaction(db);
                }
                return _BankTransaction;
            }
        }

        private IRepositoryCustomer _Customer;
        public IRepositoryCustomer Customer
        {
            get
            {
                if(_Customer == null)
                {
                    _Customer = new RepositoryCustomer(db);
                }
                return _Customer;
            }
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
