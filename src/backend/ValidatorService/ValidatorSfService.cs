using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace ValidatorService;

internal sealed class ValidatorSfService : StatelessService, IValidatorService
{
    public ValidatorSfService(StatelessServiceContext context) : base(context)
    {
    }

    public Task<ValidationResultDto> ValidateTripPlanAsync(TripPlanCreateDto request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            errors.Add("Naziv putovanja je obavezan.");
        }

        if (request.StartDate == default)
        {
            errors.Add("Pocetni datum je obavezan.");
        }

        if (request.EndDate == default)
        {
            errors.Add("Krajnji datum je obavezan.");
        }

        if (request.EndDate.Date < request.StartDate.Date)
        {
            errors.Add("Krajnji datum ne moze biti prije pocetnog datuma.");
        }

        if (request.PlannedBudget < 0)
        {
            errors.Add("Budzet ne moze biti negativan.");
        }

        return Task.FromResult(errors.Count == 0
            ? ValidationResultDto.Success()
            : ValidationResultDto.Fail(errors));
    }

    public Task<ValidationResultDto> ValidateShareAccessAsync(string token, ShareAccessType requiredAccess)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Task.FromResult(ValidationResultDto.Fail(new[] { "Share token je obavezan." }));
        }

        return Task.FromResult(ValidationResultDto.Success());
    }

    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return this.CreateServiceRemotingInstanceListeners();
    }
}
