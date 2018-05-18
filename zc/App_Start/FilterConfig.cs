using System.Web;
using System.Web.Mvc;
using zc.Filters;

namespace zc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute() { View = "~/Views/Shared/Error.cshtml" });
        }
    }
}
