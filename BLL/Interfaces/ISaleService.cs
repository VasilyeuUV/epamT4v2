using System.Collections.Generic;

namespace BLL.Interfaces
{
    interface ISaleService<T> where T : class
    {
        void SaveEntity(T entityDTO);
        T GetEntity(int? id);
        T GetEntity(string name);
        IEnumerable<T> GetEntities();
        void Dispose();
    }
}
