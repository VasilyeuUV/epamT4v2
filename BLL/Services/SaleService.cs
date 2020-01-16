using BLL.BLLModels;
using BLL.DTO;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class SaleService : IEntityService<SaleDTO>
    {
        IUnitOfWork Database { get; set; }

        public SaleService(IUnitOfWork uow)
        {
            Database = uow;
        }

        private SaleDTO ConvertToSaleDTO(Sale sale)
        {
            return new SaleDTO()
            {
                Id = sale.Id,
                DTG = sale.DTG,
                Sum = sale.Sum,
                Client = new ClientDTO() { Id = sale.Manager.Id, Name = sale.Manager.Name },
                Manager = new ManagerDTO() { Id = sale.Client.Id, Name = sale.Client.Name },
                Product = new ProductDTO() { Id = sale.Product.Id, Name = sale.Product.Name, Cost = sale.Product.Cost },
                FileName = new FileNameDTO() { Id = sale.FileName.Id, Name = sale.FileName.Name, DTG = sale.FileName.DTG }
            };
        }

        private Sale ConvertToSale(SaleDTO saleDTO)
        {
            Sale sale = new Sale()
            {
                Id = saleDTO.Id,
                DTG = saleDTO.DTG,
                Sum = saleDTO.Sum,
                Client = Database.Clients.Get(saleDTO.Client.Id)
                       ?? new Client() { Id = saleDTO.Client.Id, Name = saleDTO.Client.Name },
                Manager = Database.Managers.Get(saleDTO.Manager.Id)
                        ?? new Manager() { Id = saleDTO.Manager.Id, Name = saleDTO.Manager.Name },
                Product = Database.Products.Get(saleDTO.Product.Id)
                        ?? new Product() { Id = saleDTO.Product.Id, Name = saleDTO.Product.Name, Cost = saleDTO.Product.Cost },
                FileName = Database.FileNames.Get<FileName>(saleDTO.FileName.Name)
                        ?? new FileName() { Id = saleDTO.FileName.Id, Name = saleDTO.FileName.Name, DTG = saleDTO.FileName.DTG },
            };

            //FileName fileName = Database.FileNames.Get<FileName>(saleDTO.FileName.Name);

            //if (fileName == null)
            //{
            //    fileName = new FileName() { Id = saleDTO.FileName.Id, Name = saleDTO.FileName.Name, DTG = saleDTO.FileName.DTG };
            //}
            //sale.FileName = fileName;
            return sale;



        }


        #region ISALESERVICE
        //#################################################################################################################

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SaleDTO> GetAllEntities()
        {
            var sales = Database.Sales.GetAll();
            if (sales == null) { return null; }

            List<SaleDTO> salesDTO = new List<SaleDTO>();
            foreach (var sale in sales)
            {
                salesDTO.Add(ConvertToSaleDTO(sale));
            }
            return salesDTO.Count < 1 ? null : salesDTO;
        }

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SaleDTO GetEntity(int id = 0)
        {
            if (id == 0) { return null; }

            var sale = Database.Sales.Get(id);
            return sale == null
                         ? null
                         : ConvertToSaleDTO(sale);
        }



        /// <summary>
        /// Get entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SaleDTO GetEntity(string name)
        {
            return null;
        }

        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(SaleDTO entity)
        {
            if (entity == null) { return; }
            Sale sale = ConvertToSale(entity);

            Database.Sales.Insert(sale);
            try
            {
                Database.Save();
            }
            catch (Exception)
            {
            }
            
        }

        internal bool SaveEntities(List<SaleDTO> salesDTO)
        {
            if (salesDTO == null || salesDTO.Count < 1) { return false; }

            if (Database.FileNames.Get<FileName>(salesDTO[0].FileName.Name) == null)
            {
                Database.FileNames.Insert(new FileName() { Name = salesDTO[0].FileName.Name, DTG = salesDTO[0].FileName.DTG });
                try
                {
                    Database.Save();
                }
                catch (Exception)
                {
                    return false;
                }                
            }
            else { return false; }

            foreach (var saleDTO in salesDTO)
            {
                Sale sale = ConvertToSale(saleDTO);
                Database.Sales.Insert(sale);
            }
            try
            {
                Database.Save();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }



        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(SaleDTO entity)
        {
            if (entity == null) { return; }

            SaleDTO saleDTO = GetEntity(entity.Id);
            if (saleDTO == null)
            {
                SaveEntity(entity);
            }
            else
            {
                Sale sale = ConvertToSale(entity);
                Database.Sales.Update(sale);
                Database.Save();
            }
        }



        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(int id)
        {
            SaleDTO saleDTO = GetEntity(id);
            if (saleDTO != null)
            {
                Database.Sales.Delete(id);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(SaleDTO entity)
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
