﻿using EFCF.DataModels;
using System;

namespace EFCF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Sale> Sales { get; }
        IRepository<Manager> Managers { get; }
        IRepository<Product> Products { get; }
        IRepository<Client> Clients { get; }
        IRepository<FileName> FileNames { get; }

        void Save();
    }
}
