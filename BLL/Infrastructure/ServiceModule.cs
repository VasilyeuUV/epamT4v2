using EFCF.Interfaces;
using EFCF.Repositories;
using Ninject.Modules;

namespace BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Bind<IUnitOfWork>().To<EFUnitOfWork>();
            }
            else
            {
                Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            }            
        }
    }
}
