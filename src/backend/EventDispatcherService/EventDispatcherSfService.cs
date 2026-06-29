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
    private const string EventsQueueName = "tripPlanEvents";

    public EventDispatcherSfService(StatefulServiceContext context) : base(context)
    {
    }

    public async Task PublishAsync(TripPlanEventDto tripPlanEvent)
    {
        var queue = await StateManager.GetOrAddAsync<IReliableQueue<TripPlanEventDto>>(EventsQueueName);

        using var tx = StateManager.CreateTransaction();
        await queue.EnqueueAsync(tx, tripPlanEvent);
        await tx.CommitAsync();
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        return this.CreateServiceRemotingReplicaListeners();
    }

    protected override async Task RunAsync(CancellationToken cancellationToken)
    {
        var queue = await StateManager.GetOrAddAsync<IReliableQueue<TripPlanEventDto>>(EventsQueueName);

        while (!cancellationToken.IsCancellationRequested)
        {
            using var tx = StateManager.CreateTransaction();
            var result = await queue.TryDequeueAsync(tx);

            if (result.HasValue)
            {
                await ProcessAsync(result.Value, cancellationToken);
                await tx.CommitAsync();
                continue;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private static Task ProcessAsync(TripPlanEventDto tripPlanEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}
