using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/planovi-putovanja/{planPutovanjaId:guid}/troskovi")]
public sealed class TroskoviController : ControllerBase
{
    private const string EmailRazvojnogKorisnika = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IValidatorService validatorService;

    public TroskoviController(AppDbContext dbContext, IValidatorService validatorService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrosakDto>>> VratiSve(Guid planPutovanjaId)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var troskovi = await dbContext.Troskovi
            .AsNoTracking()
            .Where(trosak => trosak.PlanPutovanjaId == planPutovanjaId)
            .OrderByDescending(trosak => trosak.Datum)
            .ThenBy(trosak => trosak.Naziv)
            .ToListAsync();

        return Ok(troskovi.Select(TrosakMapper.UDto));
    }

    [HttpGet("pregled-budzeta")]
    public async Task<ActionResult<PregledBudzetaDto>> VratiPregledBudzeta(Guid planPutovanjaId)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null)
        {
            return NotFound();
        }

        var planPutovanja = await dbContext.PlanoviPutovanja
            .AsNoTracking()
            .Include(plan => plan.Troskovi)
            .FirstOrDefaultAsync(plan => plan.Id == planPutovanjaId && plan.UserId == korisnikId.Value);

        if (planPutovanja is null)
        {
            return NotFound();
        }

        var ukupanTrosak = planPutovanja.Troskovi.Sum(trosak => trosak.Iznos);
        var troskoviPoKategorijama = planPutovanja.Troskovi
            .GroupBy(trosak => trosak.Kategorija)
            .OrderBy(grupa => grupa.Key)
            .Select(grupa => new TrosakPoKategorijiDto
            {
                Kategorija = grupa.Key,
                UkupanIznos = grupa.Sum(trosak => trosak.Iznos)
            })
            .ToList();

        return Ok(new PregledBudzetaDto
        {
            PlanPutovanjaId = planPutovanja.Id,
            PlaniraniBudzet = planPutovanja.PlaniraniBudzet,
            UkupanTrosak = ukupanTrosak,
            PreostaliBudzet = planPutovanja.PlaniraniBudzet - ukupanTrosak,
            TroskoviPoKategorijama = troskoviPoKategorijama
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrosakDto>> VratiPoId(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var trosak = await dbContext.Troskovi
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.PlanPutovanjaId == planPutovanjaId);

        if (trosak is null)
        {
            return NotFound();
        }

        return Ok(TrosakMapper.UDto(trosak));
    }

    [HttpPost]
    public async Task<ActionResult<TrosakDto>> Kreiraj(Guid planPutovanjaId, TrosakUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajTrosakAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var trosak = TrosakMapper.UEntity(zahtjev, planPutovanjaId);
        dbContext.Troskovi.Add(trosak);
        await dbContext.SaveChangesAsync();

        var dto = TrosakMapper.UDto(trosak);
        return CreatedAtAction(nameof(VratiPoId), new { planPutovanjaId, id = trosak.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TrosakDto>> Izmijeni(Guid planPutovanjaId, Guid id, TrosakUpisDto zahtjev)
    {
        var rezultatValidacije = await validatorService.ValidirajTrosakAsync(zahtjev);
        if (!rezultatValidacije.Ispravno)
        {
            return BadRequest(rezultatValidacije);
        }

        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var trosak = await dbContext.Troskovi
            .FirstOrDefaultAsync(t => t.Id == id && t.PlanPutovanjaId == planPutovanjaId);

        if (trosak is null)
        {
            return NotFound();
        }

        TrosakMapper.AzurirajEntity(trosak, zahtjev);
        await dbContext.SaveChangesAsync();

        return Ok(TrosakMapper.UDto(trosak));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Obrisi(Guid planPutovanjaId, Guid id)
    {
        var korisnikId = await VratiIdRazvojnogKorisnikaAsync();
        if (korisnikId is null || !await PlanPripadaKorisnikuAsync(planPutovanjaId, korisnikId.Value))
        {
            return NotFound();
        }

        var trosak = await dbContext.Troskovi
            .FirstOrDefaultAsync(t => t.Id == id && t.PlanPutovanjaId == planPutovanjaId);

        if (trosak is null)
        {
            return NotFound();
        }

        dbContext.Troskovi.Remove(trosak);
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
