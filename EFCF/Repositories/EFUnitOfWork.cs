using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCF.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private SalesContext _context = null;
        private bool _disposed = false;

        private ManagerRepository _managerRepository;
        private ProductRepository _productRepository;
        private FileNameRepository _fileNameRepository;
        private ClientRepository _clientRepository;
        private SaleRepository _saleRepository;
        //private TmpSaleRepository _tmpSaleRepository;


        public EFUnitOfWork(string connectionString)
        {
            this._context = new SalesContext();
        }




        #region IUNITOFWORKS
        //################################################################################################################
               

        public void Save()
        {
            this._context.SaveChangesAsync();
        }

        public IRepository<Sale> Sales
        {
            get
            {
                if (this._saleRepository == null)
                {
                    this._saleRepository = new SaleRepository(this._context);
                }
                return this._saleRepository;
            }
        }

        //public IRepository<TmpSale> TmpSales
        //{
        //    get
        //    {
        //        if (this._tmpSaleRepository == null)
        //        {
        //            this._tmpSaleRepository = new TmpSaleRepository(this._context);
        //        }
        //        return this._tmpSaleRepository;
        //    }
        //}

        public IRepository<Manager> Managers
        {
            get
            {
                if (this._managerRepository == null)
                {
                    this._managerRepository = new ManagerRepository(this._context);
                }
                return this._managerRepository;
            }
        }

        public IRepository<Product> Products
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(this._context);
                }
                return this._productRepository;
            }
        }

        public IRepository<Client> Clients
        {
            get
            {
                if (this._clientRepository == null)
                {
                    this._clientRepository = new ClientRepository(this._context);
                }
                return this._clientRepository;
            }
        }

        public IRepository<FileName> Files
        {
            get
            {
                if (this._fileNameRepository == null)
                {
                    this._fileNameRepository = new FileNameRepository(this._context);
                }
                return this._fileNameRepository;
            }
        }

        #endregion // IUNITOFWORKS





        #region IDISPOSABLE
        //################################################################################################################

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion  // IDISPOSABLE


    }
}
