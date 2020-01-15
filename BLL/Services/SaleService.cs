using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class SaleService : ISaleService<SaleDTO>
    {
        IUnitOfWork Database { get; set; }

        public SaleService(IUnitOfWork uow)
        {
            Database = uow;
        }



        #region ISALESERVICE
        //#################################################################################################################


        public IEnumerable<SaleDTO> GetEntities()
        {
            throw new NotImplementedException();
        }

        public SaleDTO GetEntity(int? id)
        {
            throw new NotImplementedException();
        }

        public void SaveEntity(SaleDTO saleDTO)
        {
            Manager manager = Database.Managers.Get(saleDTO.ManagerId);
            Client client = new Client();
            FileName fileName = new FileName();
            Product product = new Product();

            // validation
            if (manager == null)
                throw new ValidationException("Manager not found", "");

            Sale sale = new Sale()
            {
                Client = client,
                DTG = fileName.DTG,
                FileName = fileName,
                Manager = manager,
                Product = product,
                Sum = product.Cost
            };

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public SaleDTO GetEntity(string name)
        {
            throw new NotImplementedException();
        }


        #endregion // ISALESERVICE



    }
}
