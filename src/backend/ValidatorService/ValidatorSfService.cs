using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace ValidatorService;

internal sealed class ValidatorSfService : StatelessService, IValidatorService
{
    private static readonly HashSet<string> DozvoljeniStatusiAktivnosti = new(StringComparer.OrdinalIgnoreCase)
    {
        "Planirano",
        "Rezervisano",
        "Zavrseno",
        "Otkazano"
    };

    private static readonly HashSet<string> DozvoljeneKategorijeTroskova = new(StringComparer.OrdinalIgnoreCase)
    {
        "Prevoz",
        "Smjestaj",
        "Hrana",
        "Ulaznice",
        "Kupovina",
        "Ostalo"
    };

    public ValidatorSfService(StatelessServiceContext context) : base(context)
    {
    }

    public Task<RezultatValidacijeDto> ValidirajPlanPutovanjaAsync(PlanPutovanjaUpisDto zahtjev)
    {
        var greske = new List<string>();

        if (string.IsNullOrWhiteSpace(zahtjev.Naziv))
        {
            greske.Add("Naziv putovanja je obavezan.");
        }

        if (zahtjev.PocetniDatum == default)
        {
            greske.Add("Pocetni datum je obavezan.");
        }

        if (zahtjev.KrajnjiDatum == default)
        {
            greske.Add("Krajnji datum je obavezan.");
        }

        if (zahtjev.KrajnjiDatum.Date < zahtjev.PocetniDatum.Date)
        {
            greske.Add("Krajnji datum ne moze biti prije pocetnog datuma.");
        }

        if (zahtjev.PlaniraniBudzet < 0)
        {
            greske.Add("Budzet ne moze biti negativan.");
        }

        return Task.FromResult(greske.Count == 0
            ? RezultatValidacijeDto.Uspjesno()
            : RezultatValidacijeDto.Neuspjesno(greske));
    }


    public Task<RezultatValidacijeDto> ValidirajDestinacijuAsync(DestinacijaUpisDto zahtjev)
    {
        var greske = new List<string>();

        if (string.IsNullOrWhiteSpace(zahtjev.Naziv))
        {
            greske.Add("Naziv destinacije je obavezan.");
        }

        if (string.IsNullOrWhiteSpace(zahtjev.Lokacija))
        {
            greske.Add("Lokacija destinacije je obavezna.");
        }

        if (zahtjev.DatumDolaska == default)
        {
            greske.Add("Datum dolaska je obavezan.");
        }

        if (zahtjev.DatumOdlaska == default)
        {
            greske.Add("Datum odlaska je obavezan.");
        }

        if (zahtjev.DatumOdlaska.Date < zahtjev.DatumDolaska.Date)
        {
            greske.Add("Datum odlaska ne moze biti prije datuma dolaska.");
        }

        return Task.FromResult(greske.Count == 0
            ? RezultatValidacijeDto.Uspjesno()
            : RezultatValidacijeDto.Neuspjesno(greske));
    }

    public Task<RezultatValidacijeDto> ValidirajAktivnostAsync(AktivnostUpisDto zahtjev)
    {
        var greske = new List<string>();

        if (string.IsNullOrWhiteSpace(zahtjev.Naziv))
        {
            greske.Add("Naziv aktivnosti je obavezan.");
        }

        if (zahtjev.Datum == default)
        {
            greske.Add("Datum aktivnosti je obavezan.");
        }

        if (zahtjev.ProcijenjeniTrosak < 0)
        {
            greske.Add("Procijenjeni trosak ne moze biti negativan.");
        }

        if (string.IsNullOrWhiteSpace(zahtjev.Status))
        {
            greske.Add("Status aktivnosti je obavezan.");
        }
        else if (!DozvoljeniStatusiAktivnosti.Contains(zahtjev.Status.Trim()))
        {
            greske.Add("Status aktivnosti mora biti: Planirano, Rezervisano, Zavrseno ili Otkazano.");
        }

        return Task.FromResult(greske.Count == 0
            ? RezultatValidacijeDto.Uspjesno()
            : RezultatValidacijeDto.Neuspjesno(greske));
    }

    public Task<RezultatValidacijeDto> ValidirajTrosakAsync(TrosakUpisDto zahtjev)
    {
        var greske = new List<string>();

        if (string.IsNullOrWhiteSpace(zahtjev.Naziv))
        {
            greske.Add("Naziv troska je obavezan.");
        }

        if (string.IsNullOrWhiteSpace(zahtjev.Kategorija))
        {
            greske.Add("Kategorija troska je obavezna.");
        }
        else if (!DozvoljeneKategorijeTroskova.Contains(zahtjev.Kategorija.Trim()))
        {
            greske.Add("Kategorija troska mora biti: Prevoz, Smjestaj, Hrana, Ulaznice, Kupovina ili Ostalo.");
        }

        if (zahtjev.Iznos < 0)
        {
            greske.Add("Iznos troska ne moze biti negativan.");
        }

        if (zahtjev.Datum == default)
        {
            greske.Add("Datum troska je obavezan.");
        }

        return Task.FromResult(greske.Count == 0
            ? RezultatValidacijeDto.Uspjesno()
            : RezultatValidacijeDto.Neuspjesno(greske));
    }

    public Task<RezultatValidacijeDto> ValidateShareAccessAsync(string token, ShareAccessType requiredAccess)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Task.FromResult(RezultatValidacijeDto.Neuspjesno(new[] { "Share token je obavezan." }));
        }

        return Task.FromResult(RezultatValidacijeDto.Uspjesno());
    }

    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return this.CreateServiceRemotingInstanceListeners();
    }
}
