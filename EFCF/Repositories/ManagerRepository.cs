using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class ManagerRepository : IRepository<Manager>
    {
        private SalesContext _context = null;

        public ManagerRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(Manager manager)
        {
            this._context.Managers.Add(manager);
        }

        public void Delete(int id)
        {
            Manager manager = this._context.Managers.Find(id);
            if (manager != null)
                this._context.Managers.Remove(manager);
        }

        public IEnumerable<Manager> Find(Func<Manager, bool> predicate)
        {
            return this._context.Managers.Where(predicate).ToList();
        }

        public Manager Get(int id)
        {
            return this._context.Managers.Find(id);
        }

        public IEnumerable<Manager> GetAll()
        {
            return this._context.Managers;
        }

        public void Update(Manager manager)
        {
            this._context.Entry(manager).State = EntityState.Modified;
        }
    }
}
