using EFCF.DataModels;
using System.Collections.Generic;

namespace EFCF.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        TEntity Get<TEntity>(string name) where TEntity : EntityBase;

        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
    }
}
