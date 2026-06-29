using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class PlanPutovanjaUpisDto
{
    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string? KratakOpis { get; set; }

    [DataMember]
    public DateTime PocetniDatum { get; set; }

    [DataMember]
    public DateTime KrajnjiDatum { get; set; }

    [DataMember]
    public decimal PlaniraniBudzet { get; set; }

    [DataMember]
    public string? OpsteNapomene { get; set; }
}

[DataContract]
public sealed class PlanPutovanjaDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string? KratakOpis { get; set; }

    [DataMember]
    public DateTime PocetniDatum { get; set; }

    [DataMember]
    public DateTime KrajnjiDatum { get; set; }

    [DataMember]
    public decimal PlaniraniBudzet { get; set; }

    [DataMember]
    public string? OpsteNapomene { get; set; }

    [DataMember]
    public decimal UkupanTrosak { get; set; }

    [DataMember]
    public decimal PreostaliBudzet { get; set; }

    [DataMember]
    public DateTime DatumKreiranjaUtc { get; set; }
}
