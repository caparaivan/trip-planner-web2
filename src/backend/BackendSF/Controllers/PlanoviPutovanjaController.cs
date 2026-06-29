using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/planovi-putovanja")]
public sealed class PlanoviPutovanjaController : ControllerBase
{
    private const string EmailRazvojnogKorisnika = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IEventDispatcherService eventDispatcherService;
    private readonly IValidatorService validatorService;

    public PlanoviPutovanjaController(
        AppDbContext dbContext,
        IValidatorService validatorService,
        IEventDispatcherService eventDispatcherService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
        this.eventDispatcherService = eventDispatcherService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanPutovanjaDto>>> VratiSve()
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null)
        {
            return Ok(Array.Empty<PlanPutovanjaDto>());
        }

        var planoviPutovanja = await dbContext.PlanoviPutovanja
            .AsNoTracking()
            .Include(plan => plan.Troskovi)
            .Where(plan => plan.UserId == korisnikId.Value)
            .OrderBy(plan => plan.PocetniDatum)
            .ToListAsync();

        return Ok(planoviPutovanja.Select(PlanPutovanjaMapper.UDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PlanPutovanjaDto>> VratiPoId(Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null)
        {
            return NotFound();
        }

        var planPutovanja = await dbContext.PlanoviPutovanja
            .AsNoTracking()
            .Include(plan => plan.Troskovi)
            .FirstOrDefaultAsync(plan => plan.Id == id && plan.UserId == korisnikId.Value);

        if (planPutovanja is null)
        {
            return NotFound();
        }

        return Ok(PlanPutovanjaMapper.UDto(planPutovanja));
    }

    [HttpPost]
    public async Task<ActionResult<PlanPutovanjaDto>> Kreiraj(PlanPutovanjaUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajPlanPutovanjaAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await OsigurajRazvojnogKorisnikaAsync();
        var planPutovanja = PlanPutovanjaMapper.UEntity(zahtjev, korisnikId);
        dbContext.PlanoviPutovanja.Add(planPutovanja);
        await dbContext.SaveChangesAsync();

        await eventDispatcherService.ObjaviAsync(new DogadjajPlanaPutovanjaDto
        {
            TipDogadjaja = "PlanPutovanjaKreiran",
            PlanPutovanjaId = planPutovanja.Id,
            Naziv = planPutovanja.Naziv,
            VrijemeUtc = DateTime.UtcNow
        });

        var dto = PlanPutovanjaMapper.UDto(planPutovanja);
        return CreatedAtAction(nameof(VratiPoId), new { id = planPutovanja.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PlanPutovanjaDto>> Izmijeni(Guid id, PlanPutovanjaUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajPlanPutovanjaAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null)
        {
            return NotFound();
        }

        var planPutovanja = await dbContext.PlanoviPutovanja
            .Include(plan => plan.Troskovi)
            .FirstOrDefaultAsync(plan => plan.Id == id && plan.UserId == korisnikId.Value);

        if (planPutovanja is null)
        {
            return NotFound();
        }

        PlanPutovanjaMapper.AzurirajEntity(planPutovanja, zahtjev);
        await dbContext.SaveChangesAsync();

        return Ok(PlanPutovanjaMapper.UDto(planPutovanja));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Obrisi(Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null)
        {
            return NotFound();
        }

        var planPutovanja = await dbContext.PlanoviPutovanja
            .FirstOrDefaultAsync(plan => plan.Id == id && plan.UserId == korisnikId.Value);

        if (planPutovanja is null)
        {
            return NotFound();
        }

        dbContext.PlanoviPutovanja.Remove(planPutovanja);
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

    private async Task<int> OsigurajRazvojnogKorisnikaAsync()
    {
        var postojeciKorisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (postojeciKorisnikId is not null)
        {
            return postojeciKorisnikId.Value;
        }

        var korisnik = new Entities.UserEntity
        {
            Name = "Razvojni korisnik",
            Email = EmailRazvojnogKorisnika,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("ChangeMe123!"),
            Role = "User"
        };

        dbContext.Users.Add(korisnik);
        await dbContext.SaveChangesAsync();
        return korisnik.Id;
    }
}
