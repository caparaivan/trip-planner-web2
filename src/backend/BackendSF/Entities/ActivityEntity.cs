namespace BackendSF.Entities;

public sealed class ActivityEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public TimeSpan? Time { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public decimal EstimatedCost { get; set; }

    public string Status { get; set; } = "Planned";
}
