using BLL.DTO;
using BLL.Enums;
using BLL.Services;
using EFCF.Repositories;
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


        /// <summary>
        /// CTOR
        /// </summary>
        private SaleParsingModel()
        {
        }

        /// <summary>
        /// Create SaleParsingModel instance
        /// </summary>
        /// <param name="parsedData"></param>
        /// <returns></returns>
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
            sale.Product = GetProduct(parsedData["Product"], parsedData["Cost"]);

            if (sale.Client == null
                || sale.DTG == new DateTime()
                || sale.Product == null
                || sale.Sum < 0)
            {
                sale = null;
            }
            return sale;
        }

        /// <summary>
        /// Get or create new ProductDTO object
        /// </summary>
        /// <param name="name">product name</param>
        /// <param name="sum">product cost</param>
        /// <returns></returns>
        private static ProductDTO GetProduct(string name, string sum)
        {            
            ProductService service = new ProductService(new EFUnitOfWork());
            ProductDTO product = new ProductDTO();
            try
            {
                product = service.GetEntity(name);
                if (product == null)
                {
                    product = new ProductDTO(name);
                    try
                    {
                        int cost = Convert.ToInt32(sum);
                        if (cost < 0) { throw new Exception(); }
                        product.Cost = cost;
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }  
                    service.SaveEntity(product);
                    product = service.GetEntity(name);
                }
            }
            catch (Exception)
            {
                product = null;
            }
            service.Dispose();
            return product;
        }

        /// <summary>
        /// Get or Create new ClientDTO object
        /// </summary>
        /// <param name="name">client name</param>
        /// <returns></returns>
        private static ClientDTO GetClient(string name)
        {            
            ClientService service = new ClientService(new EFUnitOfWork());
            ClientDTO client = new ClientDTO();
            try
            {
                client = service.GetEntity(name);
                if (client == null)
                {
                    service.SaveEntity(new ClientDTO(name)); 
                    client = service.GetEntity(name);
                }
            }
            catch (Exception)
            {
                client = null;
            }
            service.Dispose();
            return client;
        }


        /// <summary>
        /// Get date from string
        /// </summary>
        /// <param name="date">date as string </param>
        /// <returns></returns>
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
