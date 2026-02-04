namespace API.DTO.RollItem;

public class RollItemFilterDto
{
    public int? Id { get; set; } = null;
    public float? MinWeight { get; set; } = null;
    public float? MaxWeight { get; set; } = null;
    public float? MinLength { get; set; } = null;
    public float? MaxLength { get; set; } = null;
    public DateTime? CreatedFrom { get; set; } = null;
    public DateTime? CreatedTo { get; set; } = null;

}
