using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using NServiceBus;
using Autofac;
using Autofac.Integration.WebApi;

namespace FireOnWheels.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            var x = Assembly.GetExecutingAssembly();
            ConfigureEndpoint();
        }

        private void ConfigureEndpoint()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            var endpointConfiguration = new EndpointConfiguration("FireOnWheels.Rest");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.PurgeOnStartup(true);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => {
                    customizations.ExistingLifetimeScope(container);
                });
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var updater = new ContainerBuilder();
            updater.RegisterInstance(endpoint);
            updater.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var updated = updater.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(updated);
        }
    }
}