using BackendSF.Entities;
using TripPlanner.Contracts.Dtos;

namespace BackendSF.Mapping;

public static class TripPlanMapper
{
    public static TripPlanSummaryDto ToSummaryDto(TripPlanEntity entity)
    {
        var expensesTotal = entity.Expenses.Sum(expense => expense.Amount);

        return new TripPlanSummaryDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            PlannedBudget = entity.PlannedBudget,
            ExpensesTotal = expensesTotal,
            RemainingBudget = entity.PlannedBudget - expensesTotal
        };
    }

    public static TripPlanEntity ToEntity(TripPlanCreateDto dto, int userId)
    {
        return new TripPlanEntity
        {
            UserId = userId,
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            StartDate = dto.StartDate.Date,
            EndDate = dto.EndDate.Date,
            PlannedBudget = dto.PlannedBudget,
            Notes = dto.Notes?.Trim()
        };
    }
}
