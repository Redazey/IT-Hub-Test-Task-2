using API.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class BaseEntity : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    public DateTime? DeletedOn { get; set; }
}
