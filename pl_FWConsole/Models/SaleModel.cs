using System;

namespace pl_FWConsole.Models
{
    public class SaleModel
    {
        public int Id { get; set; }
        public DateTime DTG { get; set; }
        public int Sum { get; set; }
        public ManagerModel Manager { get; set; }
        public ProductModel Product { get; set; }
        public ClientModel Client { get; set; }
        public FileNameModel FileName { get; set; }

    }
}
