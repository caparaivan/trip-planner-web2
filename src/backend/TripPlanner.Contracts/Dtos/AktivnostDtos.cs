using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class AktivnostUpisDto
{
    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public DateTime Datum { get; set; }

    [DataMember]
    public TimeSpan? Vrijeme { get; set; }

    [DataMember]
    public string? Lokacija { get; set; }

    [DataMember]
    public string? Opis { get; set; }

    [DataMember]
    public decimal ProcijenjeniTrosak { get; set; }

    [DataMember]
    public string Status { get; set; } = "Planirano";
}

[DataContract]
public sealed class AktivnostDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public DateTime Datum { get; set; }

    [DataMember]
    public TimeSpan? Vrijeme { get; set; }

    [DataMember]
    public string? Lokacija { get; set; }

    [DataMember]
    public string? Opis { get; set; }

    [DataMember]
    public decimal ProcijenjeniTrosak { get; set; }

    [DataMember]
    public string Status { get; set; } = string.Empty;
}
