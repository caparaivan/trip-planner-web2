using Microsoft.ServiceFabric.Services.Remoting;
using TripPlanner.Contracts.Dtos;

namespace TripPlanner.Contracts.Services;

public interface IValidatorService : IService
{
    Task<RezultatValidacijeDto> ValidirajPlanPutovanjaAsync(PlanPutovanjaUpisDto zahtjev);

    Task<RezultatValidacijeDto> ValidirajDestinacijuAsync(DestinacijaUpisDto zahtjev);

    Task<RezultatValidacijeDto> ValidirajAktivnostAsync(AktivnostUpisDto zahtjev);

    Task<RezultatValidacijeDto> ValidirajTrosakAsync(TrosakUpisDto zahtjev);

    Task<RezultatValidacijeDto> ValidirajStavkuCheckListeAsync(StavkaCheckListeUpisDto zahtjev);

    Task<RezultatValidacijeDto> ValidateShareAccessAsync(string token, ShareAccessType requiredAccess);
}
