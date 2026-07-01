using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/planovi-putovanja/{planPutovanjaId:guid}/checklista")]
public sealed class StavkeCheckListeController : ControllerBase
{
    private const string EmailRazvojnogKorisnika = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IValidatorService validatorService;

    public StavkeCheckListeController(AppDbContext dbContext, IValidatorService validatorService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StavkaCheckListeDto>>> VratiSve(Guid planPutovanjaId)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var stavke = await dbContext.StavkeCheckListe
            .AsNoTracking()
            .Where(stavka => stavka.PlanPutovanjaId == planPutovanjaId)
            .OrderBy(stavka => stavka.Zavrseno)
            .ThenBy(stavka => stavka.Naziv)
            .ToListAsync();

        return Ok(stavke.Select(StavkaCheckListeMapper.UDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StavkaCheckListeDto>> VratiPoId(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var stavka = await dbContext.StavkeCheckListe
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.PlanPutovanjaId == planPutovanjaId);

        if (stavka is null)
        {
            return NotFound();
        }

        return Ok(StavkaCheckListeMapper.UDto(stavka));
    }

    [HttpPost]
    public async Task<ActionResult<StavkaCheckListeDto>> Kreiraj(Guid planPutovanjaId, StavkaCheckListeUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajStavkuCheckListeAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var stavka = StavkaCheckListeMapper.UEntity(zahtjev, planPutovanjaId);
        dbContext.StavkeCheckListe.Add(stavka);
        await dbContext.SaveChangesAsync();

        var dto = StavkaCheckListeMapper.UDto(stavka);
        return CreatedAtAction(nameof(VratiPoId), new { planPutovanjaId, id = stavka.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StavkaCheckListeDto>> Izmijeni(
        Guid planPutovanjaId,
        Guid id,
        StavkaCheckListeUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajStavkuCheckListeAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var stavka = await dbContext.StavkeCheckListe
            .FirstOrDefaultAsync(s => s.Id == id && s.PlanPutovanjaId == planPutovanjaId);

        if (stavka is null)
        {
            return NotFound();
        }

        StavkaCheckListeMapper.AzurirajEntity(stavka, zahtjev);
        await dbContext.SaveChangesAsync();

        return Ok(StavkaCheckListeMapper.UDto(stavka));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Obrisi(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var stavka = await dbContext.StavkeCheckListe
            .FirstOrDefaultAsync(s => s.Id == id && s.PlanPutovanjaId == planPutovanjaId);

        if (stavka is null)
        {
            return NotFound();
        }

        dbContext.StavkeCheckListe.Remove(stavka);
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
