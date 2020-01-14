using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class SaleRepository : IRepository<Sale>
    {
        private SalesContext _context = null;

        public SaleRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(Sale sale)
        {
            this._context.Sales.Add(sale);
        }

        public void Delete(int id)
        {
            Sale sale = this._context.Sales.Find(id);
            if (sale != null) { this._context.Sales.Remove(sale); }
        }

        public IEnumerable<Sale> Find(Func<Sale, bool> predicate)
        {
            return this._context.Sales.Include(m => m.Manager)
                                      .Include(p => p.Product)
                                      .Include(f => f.FileName)
                                      .Include(c => c.Client)
                                      .Where(predicate)
                                      .ToList();
        }

        public Sale Get(int id)
        {
            //return this._context.Sales.Find(id);
            return this._context.Sales.FindAsync(id).Result;
        }

        public IEnumerable<Sale> GetAll()
        {
            return this._context.Sales.Include(m => m.Manager)
                                      .Include(p => p.Product)
                                      .Include(f => f.FileName)
                                      .Include(c => c.Client);
        }

        public void Update(Sale sale)
        {
            this._context.Entry(sale).State = EntityState.Modified;
        }
    }
}
