using pl_FWConsole.Models;
using System.Collections.Generic;

namespace pl_FWConsole.Interfaces
{
    interface IEntity
    {
        int Id { get;}
        string Name { get; }
        ICollection<SaleModel> Sales { get; }
    }
}
