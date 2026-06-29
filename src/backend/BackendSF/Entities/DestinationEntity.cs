namespace BackendSF.Entities;

public sealed class DestinationEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TripPlanId { get; set; }

    public TripPlanEntity? TripPlan { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime ArrivalDate { get; set; }

    public DateTime DepartureDate { get; set; }

    public string? Description { get; set; }
}
