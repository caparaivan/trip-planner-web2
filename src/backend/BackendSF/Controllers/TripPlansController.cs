using BackendSF.Data;
using BackendSF.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace BackendSF.Controllers;

[ApiController]
[Route("api/trip-plans")]
public sealed class TripPlansController : ControllerBase
{
    private const string DevelopmentUserEmail = "dev@example.com";
    private readonly AppDbContext dbContext;
    private readonly IEventDispatcherService eventDispatcherService;
    private readonly IValidatorService validatorService;

    public TripPlansController(
        AppDbContext dbContext,
        IValidatorService validatorService,
        IEventDispatcherService eventDispatcherService)
    {
        this.dbContext = dbContext;
        this.validatorService = validatorService;
        this.eventDispatcherService = eventDispatcherService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripPlanSummaryDto>>> GetAll()
    {
        var userId = await GetDevelopmentUserIdAsync();
        if (userId is null)
        {
            return Ok(Array.Empty<TripPlanSummaryDto>());
        }

        var plans = await dbContext.TripPlans
            .AsNoTracking()
            .Include(plan => plan.Expenses)
            .Where(plan => plan.UserId == userId.Value)
            .OrderBy(plan => plan.StartDate)
            .ToListAsync();

        return Ok(plans.Select(TripPlanMapper.ToSummaryDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TripPlanSummaryDto>> GetById(Guid id)
    {
        var userId = await GetDevelopmentUserIdAsync();
        if (userId is null)
        {
            return NotFound();
        }

        var plan = await dbContext.TripPlans
            .AsNoTracking()
            .Include(item => item.Expenses)
            .FirstOrDefaultAsync(item => item.Id == id && item.UserId == userId.Value);

        if (plan is null)
        {
            return NotFound();
        }

        return Ok(TripPlanMapper.ToSummaryDto(plan));
    }

    [HttpPost]
    public async Task<ActionResult<TripPlanSummaryDto>> Create(TripPlanCreateDto request)
    {
        var validation = await validatorService.ValidateTripPlanAsync(request);
        if (!validation.IsValid)
        {
            return BadRequest(validation);
        }

        var userId = await EnsureDevelopmentUserExistsAsync();
        var entity = TripPlanMapper.ToEntity(request, userId);
        dbContext.TripPlans.Add(entity);
        await dbContext.SaveChangesAsync();

        await eventDispatcherService.PublishAsync(new TripPlanEventDto
        {
            EventType = "TripPlanCreated",
            TripPlanId = entity.Id,
            Title = entity.Title,
            TimestampUtc = DateTime.UtcNow
        });

        var dto = TripPlanMapper.ToSummaryDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }

    private async Task<int?> GetDevelopmentUserIdAsync()
    {
        return await dbContext.Users
            .Where(user => user.Email == DevelopmentUserEmail)
            .Select(user => (int?)user.Id)
            .FirstOrDefaultAsync();
    }

    private async Task<int> EnsureDevelopmentUserExistsAsync()
    {
        var existingUserId = await GetDevelopmentUserIdAsync();
        if (existingUserId is not null)
        {
            return existingUserId.Value;
        }

        var user = new Entities.UserEntity
        {
            Name = "Development User",
            Email = DevelopmentUserEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("ChangeMe123!"),
            Role = "User"
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user.Id;
    }
}
