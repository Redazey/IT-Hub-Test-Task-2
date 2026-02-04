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
}
