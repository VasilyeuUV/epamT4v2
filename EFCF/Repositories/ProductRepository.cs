using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private SalesContext _context = null;

        public ProductRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(Product product)
        {
            this._context.Products.Add(product);
        }

        public void Delete(int id)
        {
            Product product = this._context.Products.Find(id);
            if (product != null)
                this._context.Products.Remove(product);
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return this._context.Products.Where(predicate).ToList();
        }

        public Product Get(int id)
        {
            return this._context.Products.Find(id);
        }

        public Product Get(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            return this._context.Products;
        }

        public void Update(Product product)
        {
            this._context.Entry(product).State = EntityState.Modified;
        }
    }
}
