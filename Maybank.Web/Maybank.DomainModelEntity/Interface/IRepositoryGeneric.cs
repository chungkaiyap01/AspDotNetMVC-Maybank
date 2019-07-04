using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Maybank.DomainModelEntity.Interface
{
    public interface IRepositoryGeneric<T> where T : class
    {
        void Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);

        T ReadSingle(object ID);

        IQueryable<T> ReadAll();

        void Save();
    }
}
