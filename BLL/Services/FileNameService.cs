using BLL.DTO;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class FileNameService : IEntityService<FileNameDTO>
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public FileNameService(IUnitOfWork uow)
        {
            Database = uow;
        }


        private FileNameDTO ConvertToFileNameDTO(FileName fileName)
        {
            return new FileNameDTO()
            {
                Id = fileName.Id,
                Name = fileName.Name,
                DTG = fileName.DTG
            };
        }

        private FileName ConvertToFileName(FileNameDTO entity)
        {
            return new FileName()
            {
                Id = entity.Id,
                Name = entity.Name,
                DTG = entity.DTG
            };
        }

        #region ISALESERVICE
        //#################################################################################################################

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileNameDTO> GetAllEntities()
        {
            var fileNames = Database.FileNames.GetAll();
            if (fileNames == null) { return null; }

            List<FileNameDTO> fileNamesDTO = new List<FileNameDTO>();
            foreach (var fileName in fileNames)
            {
                fileNamesDTO.Add(ConvertToFileNameDTO(fileName));
            }
            return fileNamesDTO.Count < 1 ? null : fileNamesDTO;
        }



        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileNameDTO GetEntity(int id = 0)
        {
            if (id == 0) { return null; }

            var fileName = Database.FileNames.Get(id);
            return fileName == null ? null : ConvertToFileNameDTO(fileName);
        }

        /// <summary>
        /// Get entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileNameDTO GetEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return null; }

            var fileName = Database.FileNames.Get<FileName>(name);
            return fileName == null ? null : ConvertToFileNameDTO(fileName);
        }

        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(FileNameDTO entity)
        {
            if (entity == null) { return; }
            FileName fileName = ConvertToFileName(entity);

            Database.FileNames.Insert(fileName);
            Database.Save();
        }



        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(FileNameDTO entity)
        {
            if (entity == null) { return; }

            FileNameDTO fileNameDTO = GetEntity(entity.Id);
            if (fileNameDTO == null)
            {
                SaveEntity(entity);
            }
            else
            {
                FileName fileName = ConvertToFileName(entity);
                Database.FileNames.Update(fileName);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(int id)
        {
            FileNameDTO fileNameDTO = GetEntity(id);
            if (fileNameDTO != null)
            {
                Database.FileNames.Delete(id);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(FileNameDTO entity)
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
