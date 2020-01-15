﻿using BLL.DTO;
using BLL.Services;
using EFCF.Repositories;
using FDAL.Versions;
using System;
using System.Collections.Generic;
using System.Configuration;

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


            ManagerService service = new ManagerService(new EFUnitOfWork());
            try
            {
                fnsm.Manager = service.GetEntity(fileNameStruct["Manager"]);
                if (fnsm.Manager == null)
                {
                    service.SaveEntity(new ManagerDTO(fileNameStruct["Manager"]));
                    fnsm.Manager = service.GetEntity(fileNameStruct["Manager"]);
                }
            }
            catch (Exception ex)
            {
            }

            if (fnsm.Manager == null
                || fnsm.DTG == new DateTime())
            { fnsm = null; }
            return fnsm;
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
