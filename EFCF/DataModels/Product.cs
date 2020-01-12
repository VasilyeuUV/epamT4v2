using System.ComponentModel.DataAnnotations;

namespace EFCF.DataModels
{
    public class Product : EntityBase
    {
        [Required]
        public int Cost { get; set; }
    }
}
