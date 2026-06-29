using System.Fabric;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace EventDispatcherService;

internal sealed class EventDispatcherSfService : StatefulService, IEventDispatcherService
{
    private const string NazivRedaDogadjaja = "dogadjajiPlanovaPutovanja";

    public EventDispatcherSfService(StatefulServiceContext context) : base(context)
    {
    }

    public async Task ObjaviAsync(DogadjajPlanaPutovanjaDto dogadjajPlanaPutovanja)
    {
        var red = await StateManager.GetOrAddAsync<IReliableQueue<DogadjajPlanaPutovanjaDto>>(NazivRedaDogadjaja);

        using var tx = StateManager.CreateTransaction();
        await red.EnqueueAsync(tx, dogadjajPlanaPutovanja);
        await tx.CommitAsync();
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        return this.CreateServiceRemotingReplicaListeners();
    }

    protected override async Task RunAsync(CancellationToken cancellationToken)
    {
        var red = await StateManager.GetOrAddAsync<IReliableQueue<DogadjajPlanaPutovanjaDto>>(NazivRedaDogadjaja);

        while (!cancellationToken.IsCancellationRequested)
        {
            using var tx = StateManager.CreateTransaction();
            var rezultat = await red.TryDequeueAsync(tx);

            if (rezultat.HasValue)
            {
                await ObradiAsync(rezultat.Value, cancellationToken);
                await tx.CommitAsync();
                continue;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private static Task ObradiAsync(DogadjajPlanaPutovanjaDto dogadjajPlanaPutovanja, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}
