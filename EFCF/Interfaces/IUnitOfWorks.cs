using EFCF.DataModels;
using System;

namespace EFCF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Sale> Sales { get; }
        IRepository<TmpSale> TmpSales { get; }
        IRepository<Manager> Managers { get; }
        IRepository<Product> Products { get; }
        IRepository<Client> Clients { get; }
        IRepository<FileName> Files { get; }

        void Save();
    }
}
