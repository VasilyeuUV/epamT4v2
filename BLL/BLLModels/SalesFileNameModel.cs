using BLL.DTO;
using BLL.Services;
using EFCF.Repositories;
using FDAL.Versions;
using System;
using System.Collections.Generic;

namespace BLL.BLLModels
{
    internal class SalesFileNameModel
    {
        private string _connectionString = string.Empty;

        internal string FullPath { get; set; }
        internal string FileName { get; set; }

        internal ManagerDTO Manager { get; set; }
        internal DateTime DTG { get; set; }
        public bool ExistInDB { get; private set; } = false;


        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="connectionString"></param>
        private SalesFileNameModel(string connectionString = "")
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Create SalesFileNameModel instance
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileNameStruct"></param>
        /// <returns></returns>
        internal static SalesFileNameModel CreateInstance(string filePath, IDictionary<string, string> fileNameStruct)
        {
            SaleCsvFile csvFile = SaleCsvFile.Create(filePath);
            if (csvFile == null) { return null; }

            SalesFileNameModel fnsm = new SalesFileNameModel()
            {
                FullPath = csvFile.GetPath(),
                FileName = csvFile.GetName(),
                DTG = GetDTG(fileNameStruct["DTG"])
            };

            fnsm.ExistInDB = CheckExistInDB(fnsm.FileName);
            fnsm.Manager = GetManager(fileNameStruct["Manager"]);

            if (fnsm.Manager == null
                || fnsm.DTG == new DateTime())
            { fnsm = null; }
            return fnsm;
        }

        /// <summary>
        /// Check exists filename in DB
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool CheckExistInDB(string fileName)
        {
            FileNameDTO fileNameDTO = new FileNameDTO();
            FileNameService service = new FileNameService(new EFUnitOfWork());

            try
            {
                fileNameDTO = service.GetEntity(fileName);
            }
            catch (Exception)
            {
                fileNameDTO = null;
            }
            return fileNameDTO != null;
        }



        /// <summary>
        /// Get manager data from DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ManagerDTO GetManager(string name)
        {
            ManagerDTO manager = new ManagerDTO();
            ManagerService service = new ManagerService(new EFUnitOfWork());
            try
            {
                manager = service.GetEntity(name);
                if (manager == null)
                {
                    service.SaveEntity(new ManagerDTO(name));
                    manager = service.GetEntity(name);
                }
            }
            catch (Exception)
            {
                manager = null;
            }
            service.Dispose();
            return manager;
        }



        /// <summary>
        /// Convert string to DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime GetDTG(string date)
        {
            DateTime dtg = new DateTime();
            if (!string.IsNullOrWhiteSpace(date))
            {
                try
                {
                    dtg = DateTime.ParseExact(date, "dd.MM.yyyy"
                        , System.Globalization.CultureInfo.CurrentCulture);
                }
                catch (Exception) { }
            }
            return dtg;
        }
    }
}
