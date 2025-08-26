using Autofac;
using Autofac.Integration.Mvc;
using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System.Configuration;
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

            var builder = new ContainerBuilder();

            // Register MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register your KeyVaultService as singleton
            builder.Register<IKeyVaultService>(c =>
                new KeyVaultService(
                    tenantId: ConfigurationManager.AppSettings["TenantId"],
                    clientId: ConfigurationManager.AppSettings["ClientId"],
                    clientSecret: ConfigurationManager.AppSettings["ClientSecret"],
                    vaultBaseUrl: ConfigurationManager.AppSettings["KeyVaultUrl"]))
                .SingleInstance();

            var container = builder.Build();

            // Set Autofac as MVC Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
