using System;

namespace BLL.DTO
{
    public class TmpSaleDTO
    {
        public int Id { get; set; }
        public DateTime? DTG { get; set; }
        public int Sum { get; set; }
        public string Manager { get; set; }
        public string Product { get; set; }
        public int ProductCost { get; set; }
        public string Client { get; set; }
        public string FileName { get; set; }
        public DateTime? FileNameDTG { get; set; }
    }
}
