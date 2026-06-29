namespace BackendSF.Entities;

public sealed class TripPlanEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int UserId { get; set; }

    public UserEntity? User { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal PlannedBudget { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<DestinationEntity> Destinations { get; set; } = new List<DestinationEntity>();

    public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();

    public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public ICollection<ChecklistItemEntity> ChecklistItems { get; set; } = new List<ChecklistItemEntity>();
}
