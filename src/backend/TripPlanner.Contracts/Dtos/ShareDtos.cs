using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public enum ShareAccessType
{
    [EnumMember]
    View = 0,

    [EnumMember]
    Edit = 1
}

[DataContract]
public sealed class ShareTokenCreateDto
{
    [DataMember]
    public Guid TripPlanId { get; set; }

    [DataMember]
    public ShareAccessType AccessType { get; set; }

    [DataMember]
    public DateTime ExpiresAtUtc { get; set; }
}

[DataContract]
public sealed class ShareTokenDto
{
    [DataMember]
    public string Token { get; set; } = string.Empty;

    [DataMember]
    public Guid TripPlanId { get; set; }

    [DataMember]
    public ShareAccessType AccessType { get; set; }

    [DataMember]
    public DateTime CreatedAtUtc { get; set; }

    [DataMember]
    public DateTime ExpiresAtUtc { get; set; }
}
