using pl_FWConsole.Interfaces;
using System.Collections.Generic;

namespace pl_FWConsole.Models
{
    public class ClientModel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SaleModel> Sales { get; set; }
        public ClientModel()
        {
            this.Sales = new List<SaleModel>();
        }
    }
}
