using Microsoft.ServiceFabric.Services.Remoting;
using TripPlanner.Contracts.Dtos;

namespace TripPlanner.Contracts.Services;

public interface IValidatorService : IService
{
    Task<ValidationResultDto> ValidateTripPlanAsync(TripPlanCreateDto request);

    Task<ValidationResultDto> ValidateShareAccessAsync(string token, ShareAccessType requiredAccess);
}
