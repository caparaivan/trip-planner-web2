using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class DestinacijaUpisDto
{
    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string Lokacija { get; set; } = string.Empty;

    [DataMember]
    public DateTime DatumDolaska { get; set; }

    [DataMember]
    public DateTime DatumOdlaska { get; set; }

    [DataMember]
    public string? KratakOpis { get; set; }
}

[DataContract]
public sealed class DestinacijaDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string Lokacija { get; set; } = string.Empty;

    [DataMember]
    public DateTime DatumDolaska { get; set; }

    [DataMember]
    public DateTime DatumOdlaska { get; set; }

    [DataMember]
    public string? KratakOpis { get; set; }
}
