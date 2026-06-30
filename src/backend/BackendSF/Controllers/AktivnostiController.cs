using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/planovi-putovanja/{planPutovanjaId:guid}/aktivnosti")]
public sealed class AktivnostiController : ControllerBase
{
    private const string EmailRazvojnogKorisnika = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IValidatorService validatorService;

    public AktivnostiController(AppDbContext dbContext, IValidatorService validatorService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AktivnostDto>>> VratiSve(Guid planPutovanjaId)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var aktivnosti = await dbContext.Aktivnosti
            .AsNoTracking()
            .Where(aktivnost => aktivnost.PlanPutovanjaId == planPutovanjaId)
            .OrderBy(aktivnost => aktivnost.Datum)
            .ThenBy(aktivnost => aktivnost.Vrijeme)
            .ToListAsync();

        return Ok(aktivnosti.Select(AktivnostMapper.UDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AktivnostDto>> VratiPoId(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var aktivnost = await dbContext.Aktivnosti
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.PlanPutovanjaId == planPutovanjaId);

        if (aktivnost is null)
        {
            return NotFound();
        }

        return Ok(AktivnostMapper.UDto(aktivnost));
    }

    [HttpPost]
    public async Task<ActionResult<AktivnostDto>> Kreiraj(Guid planPutovanjaId, AktivnostUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajAktivnostAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var aktivnost = AktivnostMapper.UEntity(zahtjev, planPutovanjaId);
        dbContext.Aktivnosti.Add(aktivnost);
        await dbContext.SaveChangesAsync();

        var dto = AktivnostMapper.UDto(aktivnost);
        return CreatedAtAction(nameof(VratiPoId), new { planPutovanjaId, id = aktivnost.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AktivnostDto>> Izmijeni(Guid planPutovanjaId, Guid id, AktivnostUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajAktivnostAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var aktivnost = await dbContext.Aktivnosti
            .FirstOrDefaultAsync(a => a.Id == id && a.PlanPutovanjaId == planPutovanjaId);

        if (aktivnost is null)
        {
            return NotFound();
        }

        AktivnostMapper.AzurirajEntity(aktivnost, zahtjev);
        await dbContext.SaveChangesAsync();

        return Ok(AktivnostMapper.UDto(aktivnost));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Obrisi(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var aktivnost = await dbContext.Aktivnosti
            .FirstOrDefaultAsync(a => a.Id == id && a.PlanPutovanjaId == planPutovanjaId);

        if (aktivnost is null)
        {
            return NotFound();
        }

        dbContext.Aktivnosti.Remove(aktivnost);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private async Task<int?> VratiIdRazvojnogKorisnikaAsync()
    {
        return await dbContext.Users
            .Where(korisnik => korisnik.Email == EmailRazvojnogKorisnika)
            .Select(korisnik => (int?)korisnik.Id)
            .FirstOrDefaultAsync();
    }

    private async Task<bool> PlanPripadaKorisnikuAsync(Guid planPutovanjaId, int korisnikId)
    {
        return await dbContext.PlanoviPutovanja
            .AnyAsync(plan => plan.Id == planPutovanjaId && plan.UserId == korisnikId);
    }
}
