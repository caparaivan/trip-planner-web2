using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class RezultatValidacijeDto
{
    [DataMember]
    public bool Ispravno { get; set; }

    [DataMember]
    public List<string> Greske { get; set; } = new();

    public static RezultatValidacijeDto Uspjesno()
    {
        return new RezultatValidacijeDto { Ispravno = true };
    }

    public static RezultatValidacijeDto Neuspjesno(IEnumerable<string> greske)
    {
        return new RezultatValidacijeDto
        {
            Ispravno = false,
            Greske = greske.ToList()
        };
    }
}
