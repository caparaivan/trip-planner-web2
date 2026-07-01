using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class StavkaCheckListeMapper
{
    public static StavkaCheckListeDto UDto(StavkaCheckListeEntity stavka)
    {
        return new StavkaCheckListeDto
        {
            Id = stavka.Id,
            PlanPutovanjaId = stavka.PlanPutovanjaId,
            Naziv = stavka.Naziv,
            Zavrseno = stavka.Zavrseno
        };
    }

    public static StavkaCheckListeEntity UEntity(StavkaCheckListeUpisDto dto, Guid planPutovanjaId)
    {
        return new StavkaCheckListeEntity
        {
            PlanPutovanjaId = planPutovanjaId,
            Naziv = dto.Naziv.Trim(),
            Zavrseno = dto.Zavrseno
        };
    }

    public static void AzurirajEntity(StavkaCheckListeEntity stavka, StavkaCheckListeUpisDto dto)
    {
        stavka.Naziv = dto.Naziv.Trim();
        stavka.Zavrseno = dto.Zavrseno;
    }
}
