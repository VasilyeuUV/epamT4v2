using EFCF.DataContexts;
using EFCF.DataModels;

namespace EFCF.Repositories
{
    public sealed class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(SalesContext context) : base(context) { }
    }
}
