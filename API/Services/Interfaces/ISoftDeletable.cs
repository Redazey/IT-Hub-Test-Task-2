namespace API.Models.Interfaces;

public interface ISoftDeletable
{
    DateTime? DeletedOn { get; set; }
}
