using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maybank.DomainModelEntity.Interface;
using Maybank.InfrastructurePersistent.Context;
using System.Data.Entity;

namespace Maybank.InfrastructurePersistent.Repository
{
    public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class
    {
        protected readonly AppDbContext db = new AppDbContext();

        public RepositoryGeneric(AppDbContext db)
        {
            this.db = db;
        }

        public void Add(T Entity)
        {
            db.Set<T>().Add(Entity);
        }

        public void Delete(T Entity)
        {
            db.Set<T>().Remove(Entity);
        }

        public IQueryable<T> ReadAll()
        {
            return db.Set<T>();
        }

        public T ReadSingle(object ID)
        {
            return db.Set<T>().Find(ID);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(T Entity)
        {
            db.Entry(Entity).State = EntityState.Modified;
        }
    }
}
