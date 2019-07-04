using Maybank.DomainModelEntity.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.InfrastructurePersistent.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=Maybank")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<BankAccount> BankAccount { get; set; }
        public virtual DbSet<BankTransaction> BankTransaction { get; set; }
    }
}
