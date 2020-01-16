using BLL.Interfaces;

namespace BLL.DTO
{
    public abstract class EntityDTOBase
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public EntityDTOBase() : this("") { }
        internal EntityDTOBase(string name)
        {
            this.Id = 0;
            this.Name = name;
        }
    }
}
