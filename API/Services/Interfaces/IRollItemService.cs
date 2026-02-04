using API.DTO.RollItem;

public interface IRollItemService
{
    Task<RollItemResponseDto> AddAsync(RollItemCreateDto dto);

    Task<RollItemResponseDto?> DeleteAsync(int id);

    Task<IEnumerable<RollItemResponseDto>> GetAsync(RollItemFilterDto filter);
}
