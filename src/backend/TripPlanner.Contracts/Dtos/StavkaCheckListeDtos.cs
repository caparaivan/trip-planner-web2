using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class StavkaCheckListeUpisDto
{
    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public bool Zavrseno { get; set; }
}

[DataContract]
public sealed class StavkaCheckListeDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public bool Zavrseno { get; set; }
}
