using System.ComponentModel.DataAnnotations;

namespace API.DTO.RollItem;

public class RollItemCreateDto
{
    [Required]
    public float Length { get; set; }

    [Required]
    public float Weight { get; set; }
}
