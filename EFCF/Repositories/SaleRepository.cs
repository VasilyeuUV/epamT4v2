using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public sealed class SaleRepository : RepositoryBase<Sale>
    {
        public SaleRepository(SalesContext context) : base(context) { }



        public override IEnumerable<Sale> GetAll()
        {
            return Context.Sales.Include(m => m.Manager)
                                .Include(p => p.Product)
                                .Include(f => f.FileName)
                                .Include(c => c.Client);
        }


        //public IEnumerable<Sale> Find(Func<Sale, bool> predicate)
        //{
        //    return this._context.Sales.Include(m => m.Manager)
        //                              .Include(p => p.Product)
        //                              .Include(f => f.FileName)
        //                              .Include(c => c.Client)
        //                              .Where(predicate)
        //                              .ToList();
        //}

    }
}
