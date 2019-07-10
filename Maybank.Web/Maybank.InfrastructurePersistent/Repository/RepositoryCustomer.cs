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
    public class RepositoryCustomer : RepositoryGeneric<Customer>, IRepositoryCustomer
    {
        public RepositoryCustomer(AppDbContext db) : base(db)
        {

        }

        public int LatestCustomerID()
        {
            return db.Customer.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
        }

        public Customer LoginValidation(string Username, string Password)
        {
            return db.Customer.Where(x => x.Username == Username && x.Password == Password).FirstOrDefault();
        }
    }
}
