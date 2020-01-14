using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class TmpSaleRepository : IRepository<TmpSale>
    {
        private SalesContext _context = null;

        public TmpSaleRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(TmpSale tmpSale)
        {
            this._context.TmpSales.Add(tmpSale);
        }

        public void Delete(int id)
        {
            TmpSale tmpSale = this._context.TmpSales.Find(id);
            if (tmpSale != null)
                this._context.TmpSales.Remove(tmpSale);
        }

        public IEnumerable<TmpSale> Find(Func<TmpSale, bool> predicate)
        {
            return this._context.TmpSales.Where(predicate).ToList();
        }

        public TmpSale Get(int id)
        {
            return this._context.TmpSales.Find(id);
        }

        public IEnumerable<TmpSale> GetAll()
        {
            return this._context.TmpSales;
        }

        public void Update(TmpSale tmpSale)
        {
            this._context.Entry(tmpSale).State = EntityState.Modified;
        }
    }
}
