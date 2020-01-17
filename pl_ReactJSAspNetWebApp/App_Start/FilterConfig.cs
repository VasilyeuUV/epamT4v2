using System.Web;
using System.Web.Mvc;

namespace pl_ReactJSAspNetWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
