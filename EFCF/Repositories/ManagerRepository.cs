using EFCF.DataContexts;
using EFCF.DataModels;

namespace EFCF.Repositories
{
    public sealed class ManagerRepository : RepositoryBase<Manager>
    {
        public ManagerRepository(SalesContext context) : base(context) { }
    }
}
