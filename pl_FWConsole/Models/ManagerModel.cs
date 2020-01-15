using pl_FWConsole.Interfaces;
using System.Collections.Generic;

namespace pl_FWConsole.Models
{
    public class ManagerModel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SaleModel> Sales { get; set; }
        public ManagerModel()
        {
            this.Sales = new List<SaleModel>();
        }
    }
}
