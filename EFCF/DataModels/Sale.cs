using System;
using System.ComponentModel.DataAnnotations;

namespace EFCF.DataModels
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public DateTime DTG { get; set; }

        [Required]
        public int Sum { get; set; }


        [Required]
        public Manager Manager { get; set; }


        [Required]
        public Product Product { get; set; }


        [Required]
        public Client Client { get; set; }


        [Required]
        public FileName FileName { get; set; }

    }
}
