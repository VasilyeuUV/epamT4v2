using System.ComponentModel.DataAnnotations;

namespace EFCF.DataModels
{
    public class Product : EntityBase
    {
        [
            Required,
            Range(0, int.MaxValue)
        ]
        public int Cost { get; set; }
    }
}
