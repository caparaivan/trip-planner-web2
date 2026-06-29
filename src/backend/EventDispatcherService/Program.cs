using EventDispatcherService;
using Microsoft.ServiceFabric.Services.Runtime;

await ServiceRuntime.RegisterServiceAsync("EventDispatcherServiceType", context => new EventDispatcherSfService(context));
Thread.Sleep(Timeout.Infinite);
