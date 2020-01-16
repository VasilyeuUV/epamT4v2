using EFCF.DataModels;
using System.Data.Entity;

namespace EFCF.DataContexts
{
    public class SalesDBInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SalesContext>
    {

        protected override void Seed(SalesContext context)
        {
            if (context == null) { context = new SalesContext(); }
            
            context.Database.Initialize(false);
            context.Dispose();


            //Manager manager = context.Set<Manager>()
            //             .FirstOrDefaultAsync(x => x.Name.Equals("ManagerDefault")).Result;
        }
    }
}

