using System.Web;
using System.Web.Mvc;

namespace poc_az_kv_on_legacy_dotnet_mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
