using Maybank.DomainModelEntity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.InfrastructurePersistent.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAdministrator Administrator { get; }
        IRepositoryBankAccount BankAccount { get; }
        IRepositoryBankTransaction BankTransaction { get; }
        IRepositoryCustomer Customer { get; }

        int Commit();
    }
}
