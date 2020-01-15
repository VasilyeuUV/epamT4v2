using pl_FWConsole.Interfaces;
using System;
using System.Collections.Generic;

namespace pl_FWConsole.Models
{
    public class FileNameModel : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DTG { get; set; }
        public ICollection<SaleModel> Sales { get; set; }
        public FileNameModel()
        {
            this.Sales = new List<SaleModel>();
        }
    }
}
