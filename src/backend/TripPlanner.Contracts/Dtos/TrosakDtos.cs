using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class TrosakUpisDto
{
    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string Kategorija { get; set; } = "Ostalo";

    [DataMember]
    public decimal Iznos { get; set; }

    [DataMember]
    public DateTime Datum { get; set; }

    [DataMember]
    public string? Opis { get; set; }
}

[DataContract]
public sealed class TrosakDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string Kategorija { get; set; } = string.Empty;

    [DataMember]
    public decimal Iznos { get; set; }

    [DataMember]
    public DateTime Datum { get; set; }

    [DataMember]
    public string? Opis { get; set; }
}

[DataContract]
public sealed class TrosakPoKategorijiDto
{
    [DataMember]
    public string Kategorija { get; set; } = string.Empty;

    [DataMember]
    public decimal UkupanIznos { get; set; }
}

[DataContract]
public sealed class PregledBudzetaDto
{
    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public decimal PlaniraniBudzet { get; set; }

    [DataMember]
    public decimal UkupanTrosak { get; set; }

    [DataMember]
    public decimal PreostaliBudzet { get; set; }

    [DataMember]
    public IEnumerable<TrosakPoKategorijiDto> TroskoviPoKategorijama { get; set; } = Array.Empty<TrosakPoKategorijiDto>();
}
