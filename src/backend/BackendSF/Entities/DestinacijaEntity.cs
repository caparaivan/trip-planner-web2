namespace BackendSF.Entities;

public sealed class DestinacijaEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlanPutovanjaId { get; set; }

    public PlanPutovanjaEntity? PlanPutovanja { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Lokacija { get; set; } = string.Empty;

    public DateTime DatumDolaska { get; set; }

    public DateTime DatumOdlaska { get; set; }

    public string? KratakOpis { get; set; }
}
