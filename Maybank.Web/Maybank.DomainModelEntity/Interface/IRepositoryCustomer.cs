using System;
using System.Collections.Generic;
using System.Text;
using Maybank.DomainModelEntity.Entities;

namespace Maybank.DomainModelEntity.Interface
{
    public interface IRepositoryCustomer : IRepositoryGeneric<Customer>
    {
        Customer LoginValidation(string Username, string Password);

        int LatestCustomerID();
    }
}
