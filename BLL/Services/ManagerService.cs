﻿using BLL.DTO;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ManagerService : IEntityService<ManagerDTO>
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public ManagerService(IUnitOfWork uow)
        {
            Database = uow;
        }


        private ManagerDTO ConvertToManagerDTO(Manager manager)
        {
            return new ManagerDTO() 
            { 
                Id = manager.Id, 
                Name = manager.Name 
            };
        }

        private Manager ConvertToManager(ManagerDTO entity)
        {
            return new Manager()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }


        #region ISALESERVICE
        //#################################################################################################################

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManagerDTO> GetAllEntities()
        {
            var managers = Database.Managers.GetAll();
            if (managers == null) { return null; }

            List<ManagerDTO> managersDTO = new List<ManagerDTO>();
            foreach (var manager in managers)
            {
                managersDTO.Add(ConvertToManagerDTO(manager));
            }
            return managersDTO.Count < 1 ? null : managersDTO;
        }



        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ManagerDTO GetEntity(int id = 0)
        {
            if (id == 0) { return null; }

            var manager = Database.Managers.Get(id);
            return manager == null ? null : ConvertToManagerDTO(manager);
        }

        /// <summary>
        /// Get entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ManagerDTO GetEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return null; }

            var manager = Database.Managers.Get<Manager>(name);
            return manager == null ? null : ConvertToManagerDTO(manager);
        }

        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(ManagerDTO entity)
        {
            if (entity == null) { return; }
            Manager manager = ConvertToManager(entity);
            Database.Managers.Insert(manager);
            Database.Save();
        }



        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(ManagerDTO entity)
        {
            if (entity == null) { return; }

            ManagerDTO managerDTO = GetEntity(entity.Id);
            if (managerDTO == null)
            {
                SaveEntity(entity);
            }
            else
            {
                Manager manager = ConvertToManager(entity);
                Database.Managers.Update(manager);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(int id)
        {
            ManagerDTO managerDTO = GetEntity(id);
            if (managerDTO != null)
            {
                Database.Managers.Delete(id);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(ManagerDTO entity)
        {
            DeleteEntity(entity.Id);
        }


        /// <summary>
        /// Dispose UnitOfWork
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }


        #endregion // ISALESERVICE


    }
}
