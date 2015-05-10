using Majstic.Support;
using System.Web;
using System.Web.Mvc;

namespace Majstic
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }
    }
}
