using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class TrosakMapper
{
    public static TrosakDto UDto(TrosakEntity trosak)
    {
        return new TrosakDto
        {
            Id = trosak.Id,
            PlanPutovanjaId = trosak.PlanPutovanjaId,
            Naziv = trosak.Naziv,
            Kategorija = trosak.Kategorija,
            Iznos = trosak.Iznos,
            Datum = trosak.Datum,
            Opis = trosak.Opis
        };
    }

    public static TrosakEntity UEntity(TrosakUpisDto dto, Guid planPutovanjaId)
    {
        return new TrosakEntity
        {
            PlanPutovanjaId = planPutovanjaId,
            Naziv = dto.Naziv.Trim(),
            Kategorija = dto.Kategorija.Trim(),
            Iznos = dto.Iznos,
            Datum = dto.Datum.Date,
            Opis = dto.Opis?.Trim()
        };
    }

    public static void AzurirajEntity(TrosakEntity trosak, TrosakUpisDto dto)
    {
        trosak.Naziv = dto.Naziv.Trim();
        trosak.Kategorija = dto.Kategorija.Trim();
        trosak.Iznos = dto.Iznos;
        trosak.Datum = dto.Datum.Date;
        trosak.Opis = dto.Opis?.Trim();
    }
}
