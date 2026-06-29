using Microsoft.ServiceFabric.Services.Remoting;
using TripPlanner.Contracts.Dtos;

namespace TripPlanner.Contracts.Services;

public interface IEventDispatcherService : IService
{
    Task ObjaviAsync(DogadjajPlanaPutovanjaDto dogadjajPlanaPutovanja);
}
