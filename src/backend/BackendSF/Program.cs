using BackendSF;
using Microsoft.ServiceFabric.Services.Runtime;

await ServiceRuntime.RegisterServiceAsync("BackendSFType", context => new BackendSfService(context));
Thread.Sleep(Timeout.Infinite);
