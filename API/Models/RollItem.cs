using API.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class RollItem : BaseEntity
{
    public float Length { get; set; }

    public float Weight { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    // Реализация "мягкого" удаления и PK находится в BaseEntity
}
