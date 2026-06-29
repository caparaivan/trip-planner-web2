namespace BackendSF.Entities;

public sealed class UserEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<PlanPutovanjaEntity> PlanoviPutovanja { get; set; } = new List<PlanPutovanjaEntity>();
}
