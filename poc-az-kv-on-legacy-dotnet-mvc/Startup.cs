using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using poc_az_kv_on_legacy_dotnet_mvc;
using poc_az_kv_on_legacy_dotnet_mvc.App_Start;
using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]
namespace poc_az_kv_on_legacy_dotnet_mvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            AutofacConfig.RegisterComponents(builder);

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