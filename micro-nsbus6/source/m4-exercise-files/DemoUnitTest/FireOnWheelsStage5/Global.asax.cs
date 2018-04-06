using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

namespace FireOnWheels.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureEndpoint();
        }

        private void ConfigureEndpoint()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var endpointConfiguration = new EndpointConfiguration("FireOnWheels.Web");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.PurgeOnStartup(true);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.MakeInstanceUniquelyAddressable("uniqueId");
            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => {
                    customizations.ExistingLifetimeScope(container);
                });
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var updater = new ContainerBuilder();
            updater.RegisterInstance(endpoint);
            updater.RegisterControllers(typeof(MvcApplication).Assembly);
            var updated = updater.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(updated));
        }
    }
}