using System;

namespace BLL.DTO
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public DateTime? DTG { get; set; }
        public int ManagerId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int FileNameId { get; set; }

    }
}
