using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class TripPlanEventDto
{
    [DataMember]
    public string EventType { get; set; } = string.Empty;

    [DataMember]
    public Guid TripPlanId { get; set; }

    [DataMember]
    public string Title { get; set; } = string.Empty;

    [DataMember]
    public string? Email { get; set; }

    [DataMember]
    public DateTime TimestampUtc { get; set; }
}
