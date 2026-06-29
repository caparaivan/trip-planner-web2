namespace BackendSF.Entities;

public sealed class ChecklistItemEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TripPlanId { get; set; }

    public TripPlanEntity? TripPlan { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsDone { get; set; }
}
