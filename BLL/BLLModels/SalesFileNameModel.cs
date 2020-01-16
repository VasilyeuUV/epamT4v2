﻿using BLL.DTO;
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


        private SalesFileNameModel(string connectionString = "")
        {
            this._connectionString = connectionString;
        }

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

            fnsm.Manager = GetManager(fileNameStruct["Manager"]);

            if (fnsm.Manager == null
                || fnsm.DTG == new DateTime())
            { fnsm = null; }
            return fnsm;
        }


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
