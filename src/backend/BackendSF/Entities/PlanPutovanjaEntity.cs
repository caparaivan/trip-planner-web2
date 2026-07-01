namespace BackendSF.Entities;

public sealed class PlanPutovanjaEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int UserId { get; set; }

    public UserEntity? User { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string? KratakOpis { get; set; }

    public DateTime PocetniDatum { get; set; }

    public DateTime KrajnjiDatum { get; set; }

    public decimal PlaniraniBudzet { get; set; }

    public string? OpsteNapomene { get; set; }

    public DateTime DatumKreiranjaUtc { get; set; } = DateTime.UtcNow;

    public ICollection<DestinacijaEntity> Destinacije { get; set; } = new List<DestinacijaEntity>();

    public ICollection<AktivnostEntity> Aktivnosti { get; set; } = new List<AktivnostEntity>();

    public ICollection<TrosakEntity> Troskovi { get; set; } = new List<TrosakEntity>();

    public ICollection<StavkaCheckListeEntity> StavkeCheckListe { get; set; } = new List<StavkaCheckListeEntity>();
}
