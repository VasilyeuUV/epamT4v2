using BLL.DTO;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ProductService : IEntityService<ProductDTO>
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
        }


        private ProductDTO ConvertToProductDTO(Product product)
        {
            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost
            };
        }

        private Product ConvertToProduct(ProductDTO entity)
        {
            return new Product()
            {
                Id = entity.Id,
                Name = entity.Name,
                Cost = entity.Cost
            };
        }



        #region ISALESERVICE
        //#################################################################################################################

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDTO> GetAllEntities()
        {
            var products = Database.Products.GetAll();
            if (products == null) { return null; }

            List<ProductDTO> productsDTO = new List<ProductDTO>();
            foreach (var product in products)
            {
                productsDTO.Add(ConvertToProductDTO(product));
            }
            return productsDTO.Count < 1 ? null : productsDTO;
        }



        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDTO GetEntity(int id = 0)
        {
            if (id == 0) { return null; }

            var product = Database.Products.Get(id);
            return product == null ? null : ConvertToProductDTO(product);
        }

        /// <summary>
        /// Get entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProductDTO GetEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return null; }

            var product = Database.Products.Get<Product>(name);
            return product == null ? null : ConvertToProductDTO(product);
        }

        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(ProductDTO entity)
        {
            if (entity == null) { return; }

            Product product = ConvertToProduct(entity);

            Database.Products.Insert(product);
            Database.Save();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(ProductDTO entity)
        {
            if (entity == null) { return; }

            ProductDTO productDTO = GetEntity(entity.Id);
            if (productDTO == null)
            {
                SaveEntity(entity);
            }
            else
            {
                Product product = ConvertToProduct(entity);
                Database.Products.Update(product);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(int id)
        {
            ProductDTO productDTO = GetEntity(id);
            if (productDTO != null)
            {
                Database.Products.Delete(id);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(ProductDTO entity)
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
