using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class DogadjajPlanaPutovanjaDto
{
    [DataMember]
    public string TipDogadjaja { get; set; } = string.Empty;

    [DataMember]
    public Guid PlanPutovanjaId { get; set; }

    [DataMember]
    public string Naziv { get; set; } = string.Empty;

    [DataMember]
    public string? Email { get; set; }

    [DataMember]
    public DateTime VrijemeUtc { get; set; }
}
