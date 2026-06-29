using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/planovi-putovanja/{planPutovanjaId:guid}/destinacije")]
public sealed class DestinacijeController : ControllerBase
{
    private const string EmailRazvojnogKorisnika = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IValidatorService validatorService;

    public DestinacijeController(AppDbContext dbContext, IValidatorService validatorService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DestinacijaDto>>> VratiSve(Guid planPutovanjaId)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var destinacije = await dbContext.Destinacije
            .AsNoTracking()
            .Where(destinacija => destinacija.PlanPutovanjaId == planPutovanjaId)
            .OrderBy(destinacija => destinacija.DatumDolaska)
            .ToListAsync();

        return Ok(destinacije.Select(DestinacijaMapper.UDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DestinacijaDto>> VratiPoId(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var destinacija = await dbContext.Destinacije
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id && d.PlanPutovanjaId == planPutovanjaId);

        if (destinacija is null)
        {
            return NotFound();
        }

        return Ok(DestinacijaMapper.UDto(destinacija));
    }

    [HttpPost]
    public async Task<ActionResult<DestinacijaDto>> Kreiraj(Guid planPutovanjaId, DestinacijaUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajDestinacijuAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var destinacija = DestinacijaMapper.UEntity(zahtjev, planPutovanjaId);
        dbContext.Destinacije.Add(destinacija);
        await dbContext.SaveChangesAsync();

        var dto = DestinacijaMapper.UDto(destinacija);
        return CreatedAtAction(nameof(VratiPoId), new { planPutovanjaId, id = destinacija.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DestinacijaDto>> Izmijeni(Guid planPutovanjaId, Guid id, DestinacijaUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajDestinacijuAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var destinacija = await dbContext.Destinacije
            .FirstOrDefaultAsync(d => d.Id == id && d.PlanPutovanjaId == planPutovanjaId);

        if (destinacija is null)
        {
            return NotFound();
        }

        DestinacijaMapper.AzurirajEntity(destinacija, zahtjev);
        await dbContext.SaveChangesAsync();

        return Ok(DestinacijaMapper.UDto(destinacija));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Obrisi(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var destinacija = await dbContext.Destinacije
            .FirstOrDefaultAsync(d => d.Id == id && d.PlanPutovanjaId == planPutovanjaId);

        if (destinacija is null)
        {
            return NotFound();
        }

        dbContext.Destinacije.Remove(destinacija);
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
