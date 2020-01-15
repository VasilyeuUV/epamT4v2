using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ManagerService : ISaleService<ManagerDTO>
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public ManagerService(IUnitOfWork uow)
        {
            Database = uow;
        }





        #region ISALESERVICE
        //#################################################################################################################


        public void SaveEntity(ManagerDTO managerDTO)
        {            
        }

        public ManagerDTO GetEntity(int id = 0)
        {
            if (id == 0)
            {
                throw new ValidationException("Need to specify manager id or manager name", "");
            }

            var manager = Database.Managers.Get(id);
            if (manager == null)
            {
                throw new ValidationException("This manager is not registered", "");
            }

            return new ManagerDTO() { Id = manager.Id, Name = manager.Name };
        }

        public ManagerDTO GetEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Need to specify manager id or manager name", "");
            }

            var manager = Database.Managers.Get(name);
            if (manager == null)
            {
                throw new ValidationException("This manager is not registered", "");
            }
            return new ManagerDTO() { Id = manager.Id, Name = manager.Name };
        }


        public IEnumerable<ManagerDTO> GetEntities()
        {

            return null;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public ManagerDTO GetEntity(int? id)
        {
            throw new NotImplementedException();
        }





        #endregion // ISALESERVICE


    }
}
