namespace BLL.DTO
{
    public class ProductDTO : EntityDTOBase
    {
        public int Cost { get; set; }

        public ProductDTO() : this("") { }
        internal ProductDTO(string name) : base(name) 
        {
            this.Cost = 0;
        }
    }
}
