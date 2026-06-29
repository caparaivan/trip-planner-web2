using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class PlanPutovanjaMapper
{
    public static PlanPutovanjaDto UDto(PlanPutovanjaEntity planPutovanja)
    {
        var ukupanTrosak = planPutovanja.Troskovi.Sum(trosak => trosak.Amount);

        return new PlanPutovanjaDto
        {
            Id = planPutovanja.Id,
            Naziv = planPutovanja.Naziv,
            KratakOpis = planPutovanja.KratakOpis,
            PocetniDatum = planPutovanja.PocetniDatum,
            KrajnjiDatum = planPutovanja.KrajnjiDatum,
            PlaniraniBudzet = planPutovanja.PlaniraniBudzet,
            OpsteNapomene = planPutovanja.OpsteNapomene,
            UkupanTrosak = ukupanTrosak,
            PreostaliBudzet = planPutovanja.PlaniraniBudzet - ukupanTrosak,
            DatumKreiranjaUtc = planPutovanja.DatumKreiranjaUtc
        };
    }

    public static PlanPutovanjaEntity UEntity(PlanPutovanjaUpisDto dto, int korisnikId)
    {
        return new PlanPutovanjaEntity
        {
            UserId = korisnikId,
            Naziv = dto.Naziv.Trim(),
            KratakOpis = dto.KratakOpis?.Trim(),
            PocetniDatum = dto.PocetniDatum.Date,
            KrajnjiDatum = dto.KrajnjiDatum.Date,
            PlaniraniBudzet = dto.PlaniraniBudzet,
            OpsteNapomene = dto.OpsteNapomene?.Trim()
        };
    }

    public static void AzurirajEntity(PlanPutovanjaEntity planPutovanja, PlanPutovanjaUpisDto dto)
    {
        planPutovanja.Naziv = dto.Naziv.Trim();
        planPutovanja.KratakOpis = dto.KratakOpis?.Trim();
        planPutovanja.PocetniDatum = dto.PocetniDatum.Date;
        planPutovanja.KrajnjiDatum = dto.KrajnjiDatum.Date;
        planPutovanja.PlaniraniBudzet = dto.PlaniraniBudzet;
        planPutovanja.OpsteNapomene = dto.OpsteNapomene?.Trim();
    }
}
