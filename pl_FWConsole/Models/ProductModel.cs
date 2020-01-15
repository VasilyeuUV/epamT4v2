using pl_FWConsole.Interfaces;
using System.Collections.Generic;

namespace pl_FWConsole.Models
{
    public class ProductModel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public ICollection<SaleModel> Sales { get; set; }
        public ProductModel()
        {
            this.Sales = new List<SaleModel>();
        }        
    }
}
