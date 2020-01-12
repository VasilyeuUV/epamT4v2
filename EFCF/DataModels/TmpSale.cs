using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCF.DataModels
{
    public class TmpSale
    {
        public int Id { get; set; }

        [Required]
        public DateTime DTG { get; set; }

        [Required]
        public int Sum { get; set; }

        [
        Required,
        MinLength(1, ErrorMessage = "Name must be 2 characters or more"),
        MaxLength(100, ErrorMessage = "Name must be 100 characters or less"),
        ]
        public Manager Manager { get; set; }

        [
        Required,
        MinLength(1, ErrorMessage = "Name must be 2 characters or more"),
        MaxLength(100, ErrorMessage = "Name must be 100 characters or less"),
        ]
        public Product Product { get; set; }

        [
        Required,
        MinLength(1, ErrorMessage = "Name must be 2 characters or more"),
        MaxLength(100, ErrorMessage = "Name must be 100 characters or less"),
        ]
        public Client Client { get; set; }

        [
        Required,
        MinLength(16, ErrorMessage = "Name must be 16 characters or more"),
        MaxLength(100, ErrorMessage = "Name must be 100 characters or less"),
        ]
        public FileName FileName { get; set; }

    }
}
