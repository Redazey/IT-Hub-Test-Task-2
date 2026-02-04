namespace API.DTO.RollItem;

public class RollItemResponseDto
{
    public int Id { get; set; }

    public float Length { get; set; }

    public float Weight { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
}
