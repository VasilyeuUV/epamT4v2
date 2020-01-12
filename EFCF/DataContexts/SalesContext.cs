namespace EFCF.DataContexts
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SalesContext : DbContext
    {
        public SalesContext()
            : base("name=SalesContext")
        {
        }
}