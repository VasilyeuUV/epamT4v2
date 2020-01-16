using BLL.DTO;
using BLL.Enums;
using BLL.Services;
using System;
using System.Collections.Generic;

namespace BLL.BLLModels
{
    internal class SaleParsingModel
    {

        internal int Sum { get; set; }
        internal DateTime DTG { get; set; }
        internal ProductDTO Product { get; set; }
        internal ClientDTO Client { get; set; }

        private SaleParsingModel()
        {
        }

        internal static SaleParsingModel CreateInstance(IDictionary<string, string> parsedData)
        {
            if (parsedData == null 
                || parsedData.Count < 1
                || string.IsNullOrWhiteSpace(parsedData["Client"])
                || string.IsNullOrWhiteSpace(parsedData["Cost"])
                || string.IsNullOrWhiteSpace(parsedData["DTG"])
                || string.IsNullOrWhiteSpace(parsedData["Product"])
                )
            {
                //OnErrorEvent(EnumErrors.fileContentError, "Parse data = null");
                return null;
            }
                       
            SaleParsingModel sale = new SaleParsingModel();
            
            try { sale.Sum = Convert.ToInt32(parsedData["Cost"]); }
            catch (Exception ex) 
            {
                /*OnErrorEvent(EnumErrors.costError, ex.Message);*/
                return null;
            }

            sale.DTG = GetDTG(parsedData["DTG"]);
            if (sale.DTG == new DateTime()) 
            {
                /*OnErrorEvent(EnumErrors.dateError, ex.Message);*/
                return null;
            }

            sale.Client = GetClient(parsedData["Client"]);
            sale.Product = GetProduct(parsedData["Client"]);

            //sale.Client = GetClientFromDB(repo, parsedData["Client"])
            //              ?? new Client() { Name = parsedData["Client"] };
            //sale.Product = GetProductFromDB(repo, parsedData["Product"], this._checkProductsDB)
            //              ?? new Product() { Name = parsedData["Product"], Cost = sale.Sum };


            if (sale.Client == null
                || sale.DTG == new DateTime()
                || sale.Product == null
                || sale.Sum < 0)
            {
                sale = null;
            }
            return sale;
        }

        private static ManagerDTO GetClient(string name)
        {
            ClientDTO client = new ClientDTO();
            ManagerService service = new ManagerService(new EFUnitOfWork());
            try
            {
                client = service.GetEntity(name);
                if (client == null)
                {
                    service.SaveEntity(new ManagerDTO(name));
                    client = service.GetEntity(name);
                }
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }



        private static DateTime GetDTG(string date)
        {
            DateTime dtg = new DateTime();
            if (!string.IsNullOrWhiteSpace(date))
            {
                try
                {
                    dtg = DateTime.ParseExact(date, "dd.MM.yyyy HH:mm:ss"
                        , System.Globalization.CultureInfo.CurrentCulture);
                }
                catch (Exception) { }
            }
            return dtg;
        }


    }
}
