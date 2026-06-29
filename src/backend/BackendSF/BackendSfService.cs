using System.Fabric;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace BackendSF;

internal sealed class BackendSfService : StatelessService
{
    public BackendSfService(StatelessServiceContext context) : base(context)
    {
    }

    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return new[]
        {
            new ServiceInstanceListener(serviceContext =>
                new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                {
                    return Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                        .UseKestrel()
                        .ConfigureServices(services => services.AddSingleton(serviceContext))
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>()
                        .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                        .UseUrls(url)
                        .Build();
                }))
        };
    }
}
