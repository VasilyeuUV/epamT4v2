namespace BLL.DTO
{
    public class ManagerDTO
    {
        public ManagerDTO() : this("") { }
        internal ManagerDTO(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
