using Autofac;
using Autofac.Integration.Mvc;
using poc_az_kv_on_legacy_dotnet_mvc.App_Start;
using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace poc_az_kv_on_legacy_dotnet_mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
