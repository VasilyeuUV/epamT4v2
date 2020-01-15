using System.Configuration;

namespace pl_FWConsole.Configurations
{
    internal class SaleConfiguration
    {
        internal string ManagerSing { get; private set; }
        internal string ProductSing { get; private set; }
        internal string ClientSing { get; private set; }
        internal string DataTimeSing { get; private set; }
        internal string SumSing { get; private set; }

        public SaleConfiguration()
        {
            this.ClientSing = ConfigurationManager.AppSettings["ClientSing"];
            this.ManagerSing = ConfigurationManager.AppSettings["ManagerSing"];
            this.ProductSing = ConfigurationManager.AppSettings["ProductSing"];
            this.DataTimeSing = ConfigurationManager.AppSettings["DataTimeSing"];
            this.SumSing = ConfigurationManager.AppSettings["SumSing"];
        }


    }
}
