using Autofac;
using Autofac.Integration.Mvc;
using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace poc_az_kv_on_legacy_dotnet_mvc.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterComponents(ContainerBuilder builder)
        {
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


        }
    }
}