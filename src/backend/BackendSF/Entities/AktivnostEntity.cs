namespace BackendSF.Entities;

public sealed class AktivnostEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public DateTime Datum { get; set; }

    public TimeSpan? Vrijeme { get; set; }

    public string? Lokacija { get; set; }

    public string? Opis { get; set; }

    public decimal ProcijenjeniTrosak { get; set; }

    public string Status { get; set; } = "Planirano";
}
