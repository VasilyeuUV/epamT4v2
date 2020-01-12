using System.Data.Entity;

namespace DAL.DataContexts
{
    public class SalesContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<FileName> Files { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<TmpSale> TmpSales { get; set; }

        public SalesContext()
            : base("name=SalesContext")
        {
        }
    }
}
