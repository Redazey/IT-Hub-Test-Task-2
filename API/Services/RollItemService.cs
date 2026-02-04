using API.Data;
using API.DTO.RollItem;
using API.Extensions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class RollItemService : IRollItemService
{
    private readonly AppDbContext _context;

    public RollItemService(AppDbContext context)
    {
        _context = context;
    }

    private static RollItemResponseDto Map(RollItem item)
    {
        return new RollItemResponseDto
        {
            Id = item.Id,
            Length = item.Length,
            Weight = item.Weight,
            CreatedOn = item.CreatedOn,
            DeletedOn = item.DeletedOn
        };
    }

    public async Task<RollItemResponseDto> AddAsync(RollItemCreateDto dto)
    {
        var item = new RollItem
        {
            Length = dto.Length,
            Weight = dto.Weight
        };

        _context.RollItems.Add(item);
        await _context.SaveChangesAsync();

        return Map(item);
    }

    public async Task<RollItemResponseDto?> DeleteAsync(int id)
    {
        var item = await _context.RollItems.FindAsync(id);

        if (item == null)
            return null;

        item.DeletedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Map(item);
    }

    public async Task<IEnumerable<RollItemResponseDto>> GetAsync(RollItemFilterDto filter)
    {
        IQueryable<RollItem> query = _context.RollItems.AsQueryable();

        query = query
            .WhereIf(filter.Id.HasValue, x => x.Id == filter.Id)
            .WhereIf(filter.MinWeight.HasValue, x => x.Weight >= filter.MinWeight)
            .WhereIf(filter.MaxWeight.HasValue, x => x.Weight <= filter.MaxWeight)
            .WhereIf(filter.MinLength.HasValue, x => x.Length >= filter.MinLength)
            .WhereIf(filter.MaxLength.HasValue, x => x.Length <= filter.MaxLength)
            .WhereIf(filter.CreatedFrom.HasValue, x => x.CreatedOn >= filter.CreatedFrom)
            .WhereIf(filter.CreatedTo.HasValue, x => x.CreatedOn <= filter.CreatedTo);

        var items = await query.ToListAsync();

        return items.Select(Map);
    }

    public async Task<RollItemStatsResponseDto> GetStatsAsync(RollItemStatsFilterDto filter)
    {
        var allItems = await _context.RollItems
            .IgnoreQueryFilters()
            .Where(x => x.CreatedOn >= filter.From && x.CreatedOn <= filter.To)
            .ToListAsync();

        if (!allItems.Any())
            return new RollItemStatsResponseDto();

        var activeItems = allItems.Where(x => x.DeletedOn == null).ToList();
        var deletedItems = allItems.Where(x => x.DeletedOn != null && x.DeletedOn >= filter.From && x.DeletedOn <= filter.To).ToList();

        var durations = allItems
            .Where(x => x.DeletedOn.HasValue)
            .Select(x => x.DeletedOn.Value - x.CreatedOn)
            .ToList();

        return new RollItemStatsResponseDto
        {
            AddedCount = allItems.Count,
            DeletedCount = deletedItems.Count,

            AvgLength = activeItems.Any() ? activeItems.Average(x => x.Length) : 0,
            AvgWeight = activeItems.Any() ? activeItems.Average(x => x.Weight) : 0,

            MaxLength = activeItems.Any() ? activeItems.Max(x => x.Length) : 0,
            MinLength = activeItems.Any() ? activeItems.Min(x => x.Length) : 0,

            MaxWeight = activeItems.Any() ? activeItems.Max(x => x.Weight) : 0,
            MinWeight = activeItems.Any() ? activeItems.Min(x => x.Weight) : 0,

            TotalWeight = activeItems.Any() ? activeItems.Sum(x => x.Weight) : 0,

            MaxDuration = durations.Any() ? durations.Max() : null,
            MinDuration = durations.Any() ? durations.Min() : null
        };
    }
}
