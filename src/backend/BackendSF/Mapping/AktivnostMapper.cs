using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class AktivnostMapper
{
    public static AktivnostDto UDto(AktivnostEntity aktivnost)
    {
        return new AktivnostDto
        {
            Id = aktivnost.Id,
            PlanPutovanjaId = aktivnost.PlanPutovanjaId,
            Naziv = aktivnost.Naziv,
            Datum = aktivnost.Datum,
            Vrijeme = aktivnost.Vrijeme,
            Lokacija = aktivnost.Lokacija,
            Opis = aktivnost.Opis,
            ProcijenjeniTrosak = aktivnost.ProcijenjeniTrosak,
            Status = aktivnost.Status
        };
    }

    public static AktivnostEntity UEntity(AktivnostUpisDto dto, Guid planPutovanjaId)
    {
        return new AktivnostEntity
        {
            PlanPutovanjaId = planPutovanjaId,
            Naziv = dto.Naziv.Trim(),
            Datum = dto.Datum.Date,
            Vrijeme = dto.Vrijeme,
            Lokacija = dto.Lokacija?.Trim(),
            Opis = dto.Opis?.Trim(),
            ProcijenjeniTrosak = dto.ProcijenjeniTrosak,
            Status = dto.Status.Trim()
        };
    }

    public static void AzurirajEntity(AktivnostEntity aktivnost, AktivnostUpisDto dto)
    {
        aktivnost.Naziv = dto.Naziv.Trim();
        aktivnost.Datum = dto.Datum.Date;
        aktivnost.Vrijeme = dto.Vrijeme;
        aktivnost.Lokacija = dto.Lokacija?.Trim();
        aktivnost.Opis = dto.Opis?.Trim();
        aktivnost.ProcijenjeniTrosak = dto.ProcijenjeniTrosak;
        aktivnost.Status = dto.Status.Trim();
    }
}
