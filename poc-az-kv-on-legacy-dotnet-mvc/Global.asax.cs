using Autofac;
using Autofac.Integration.Mvc;
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

            var builder = new ContainerBuilder();

            // Register MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register your KeyVaultService as singleton
            var section = (NameValueCollection)ConfigurationManager.GetSection("keyVaultSettings");
            builder.Register<IKeyVaultService>(c =>
                new KeyVaultService(
                    tenantId: Environment.GetEnvironmentVariable("TenantId") ?? section["TenantId"],
                    clientId: Environment.GetEnvironmentVariable("ClientId") ?? section["ClientId"],
                    clientSecret: Environment.GetEnvironmentVariable("ClientSecret") ?? section["ClientSecret"],
                    vaultBaseUrl: Environment.GetEnvironmentVariable("KeyVaultUrl") ?? section["Url"]))
                .SingleInstance();

            var container = builder.Build();

            // Set Autofac as MVC Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Resolve IKeyVaultService and get secret
            var keyVaultService = container.Resolve<IKeyVaultService>();

            // Replace connection string
            ReplaceConfigValue("DefaultConnection", keyVaultService, isConnectionString: true);

            // Replace app setting:
            ReplaceConfigValue("MyAppSettingKey", keyVaultService, isConnectionString: false);
        }

        private void ReplaceConfigValue(string key, IKeyVaultService keyVaultService, bool isConnectionString)
        {
            string secretValue = keyVaultService.GetSecretAsync(key).GetAwaiter().GetResult();
            if (isConnectionString)
            {
                var settings = ConfigurationManager.ConnectionStrings[key];
                if (settings != null)
                {
                    var field = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    field?.SetValue(settings, false);
                    settings.ConnectionString = secretValue;
                }
            }
            else
            {
                ConfigurationManager.AppSettings[key] = secretValue;
            }
        }
    }
}
