namespace BackendSF.Entities;

public sealed class TrosakEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Kategorija { get; set; } = "Ostalo";

    public decimal Iznos { get; set; }

    public DateTime Datum { get; set; }

    public string? Opis { get; set; }
}
