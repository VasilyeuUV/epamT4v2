using System;

namespace BLL.DTO
{
    public class FileNameDTO : EntityDTOBase
    {
        public DateTime DTG { get; set; }

        public FileNameDTO() : this("") { }
        internal FileNameDTO(string name) : base(name) 
        {
            this.DTG = new DateTime();
        }
    }
}
