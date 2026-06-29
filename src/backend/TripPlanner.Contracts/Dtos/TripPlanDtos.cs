using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class TripPlanCreateDto
{
    [DataMember]
    public string Title { get; set; } = string.Empty;

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public DateTime StartDate { get; set; }

    [DataMember]
    public DateTime EndDate { get; set; }

    [DataMember]
    public decimal PlannedBudget { get; set; }

    [DataMember]
    public string? Notes { get; set; }
}

[DataContract]
public sealed class TripPlanSummaryDto
{
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Title { get; set; } = string.Empty;

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public DateTime StartDate { get; set; }

    [DataMember]
    public DateTime EndDate { get; set; }

    [DataMember]
    public decimal PlannedBudget { get; set; }

    [DataMember]
    public decimal ExpensesTotal { get; set; }

    [DataMember]
    public decimal RemainingBudget { get; set; }
}
