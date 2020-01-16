using BLL.Interfaces;
using System;

namespace BLL.DTO
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public DateTime DTG { get; set; }
        public int Sum { get; set; }
        public ManagerDTO Manager { get; set; }
        public ProductDTO Product { get; set; }
        public ClientDTO Client { get; set; }
        public FileNameDTO FileName { get; set; }

    }
}
