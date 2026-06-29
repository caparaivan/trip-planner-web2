using Microsoft.ServiceFabric.Services.Runtime;
using ShareService;

await ServiceRuntime.RegisterServiceAsync("ShareServiceType", context => new ShareSfService(context));
Thread.Sleep(Timeout.Infinite);
