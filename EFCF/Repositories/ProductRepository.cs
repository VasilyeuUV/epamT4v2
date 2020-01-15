using EFCF.DataContexts;
using EFCF.DataModels;

namespace EFCF.Repositories
{
    public sealed class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(SalesContext context) : base(context) { }
    }
}
