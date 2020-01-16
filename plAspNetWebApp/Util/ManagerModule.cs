using BLL.DTO;
using BLL.Interfaces;
using BLL.Services;
using Ninject.Modules;

namespace plAspNetWebApp.Util
{
    public class ManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEntityService<ManagerDTO>>().To<ManagerService>();
        }
    }
}