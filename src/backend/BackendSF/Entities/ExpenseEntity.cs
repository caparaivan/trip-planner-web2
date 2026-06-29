namespace BackendSF.Entities;

public sealed class ExpenseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = "Other";

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }
}
