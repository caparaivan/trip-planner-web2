using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class DestinacijaMapper
{
    public static DestinacijaDto UDto(DestinacijaEntity destinacija)
    {
        return new DestinacijaDto
        {
            Id = destinacija.Id,
            PlanPutovanjaId = destinacija.PlanPutovanjaId,
            Naziv = destinacija.Naziv,
            Lokacija = destinacija.Lokacija,
            DatumDolaska = destinacija.DatumDolaska,
            DatumOdlaska = destinacija.DatumOdlaska,
            KratakOpis = destinacija.KratakOpis
        };
    }

    public static DestinacijaEntity UEntity(DestinacijaUpisDto dto, Guid planPutovanjaId)
    {
        return new DestinacijaEntity
        {
            PlanPutovanjaId = planPutovanjaId,
            Naziv = dto.Naziv.Trim(),
            Lokacija = dto.Lokacija.Trim(),
            DatumDolaska = dto.DatumDolaska.Date,
            DatumOdlaska = dto.DatumOdlaska.Date,
            KratakOpis = dto.KratakOpis?.Trim()
        };
    }

    public static void AzurirajEntity(DestinacijaEntity destinacija, DestinacijaUpisDto dto)
    {
        destinacija.Naziv = dto.Naziv.Trim();
        destinacija.Lokacija = dto.Lokacija.Trim();
        destinacija.DatumDolaska = dto.DatumDolaska.Date;
        destinacija.DatumOdlaska = dto.DatumOdlaska.Date;
        destinacija.KratakOpis = dto.KratakOpis?.Trim();
    }
}
