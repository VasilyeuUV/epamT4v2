using EFCF.DataContexts;
using EFCF.DataModels;

namespace EFCF.Repositories
{
    public sealed class FileNameRepository : RepositoryBase<FileName>
    {
        public FileNameRepository(SalesContext context) : base(context) { }
    }
}
