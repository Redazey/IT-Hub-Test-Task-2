namespace API.DTO.RollItem;

public class RollItemStatsResponseDto
{
    public int AddedCount { get; set; }
    public int DeletedCount { get; set; }

    public float AvgLength { get; set; }
    public float AvgWeight { get; set; }

    public float MaxLength { get; set; }
    public float MinLength { get; set; }

    public float MaxWeight { get; set; }
    public float MinWeight { get; set; }

    public float TotalWeight { get; set; }

    public TimeSpan? MaxDuration { get; set; }
    public TimeSpan? MinDuration { get; set; }
}
