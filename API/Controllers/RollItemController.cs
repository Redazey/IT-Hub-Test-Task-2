using API.DTO.RollItem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RollItemController : ControllerBase
{
    private readonly IRollItemService _service;

    public RollItemController(IRollItemService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<RollItemResponseDto>> Post(
        [FromBody] RollItemCreateDto dto)
    {
        var item = await _service.AddAsync(dto);

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RollItemResponseDto>> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (deleted == null)
            return NotFound();

        return Ok(deleted);
    }

    [HttpGet]
    public async Task<IEnumerable<RollItemResponseDto>> Get(
    [FromQuery] RollItemFilterDto filter)
    {
        return await _service.GetAsync(filter);
    }
}
