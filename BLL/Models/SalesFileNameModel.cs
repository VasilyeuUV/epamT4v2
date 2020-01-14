using FDAL.Versions;
using System;
using System.Collections.Generic;

namespace BLL.Models
{
    internal class SalesFileNameModel
    {

        internal string FullPath { get; set; }
        internal string FileName { get; set; }

        internal string Manager { get; set; }
        internal DateTime DTG { get; set; }


        private SalesFileNameModel()
        {            
        }

        internal static SalesFileNameModel CreateInstance(string filePath, IDictionary<string, string> fileNameStruct)
        {
            SaleCsvFile csvFile = SaleCsvFile.Create(filePath);
            if (csvFile == null) { return null; }

            SalesFileNameModel fnsm = new SalesFileNameModel
            {
                FullPath = csvFile.GetPath(),
                FileName = csvFile.GetName(),
                Manager = fileNameStruct["Manager"],
                DTG = GetDTG(fileNameStruct["DTG"])
            };

            if (string.IsNullOrWhiteSpace(fnsm.Manager)
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
