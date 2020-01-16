using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IEntityService<T> where T : class
    {
        IEnumerable<T> GetAllEntities();
        T GetEntity(int id);
        T GetEntity(string name);

        void SaveEntity(T entityDTO);
        void UpdateEntity(T entity);
        void DeleteEntity(int id);
        void DeleteEntity(T entity);

        void Dispose();

    }
}
