using Microsoft.ServiceFabric.Services.Runtime;
using ValidatorService;

await ServiceRuntime.RegisterServiceAsync("ValidatorServiceType", context => new ValidatorSfService(context));
Thread.Sleep(Timeout.Infinite);
