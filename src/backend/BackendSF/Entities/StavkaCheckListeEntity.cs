namespace BackendSF.Entities;

public sealed class StavkaCheckListeEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public bool Zavrseno { get; set; }
}
