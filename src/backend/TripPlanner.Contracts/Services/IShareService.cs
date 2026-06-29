using Microsoft.ServiceFabric.Services.Remoting;
using TripPlanner.Contracts.Dtos;

namespace TripPlanner.Contracts.Services;

public interface IShareService : IService
{
    Task<ShareTokenDto> CreateShareTokenAsync(ShareTokenCreateDto request);

    Task<ShareTokenDto?> GetShareTokenAsync(string token);
}
