using Domain.Interfaces;

namespace Domain.Types;

public class ThreadEntity : IEntity<int>
{
    public int PKey { get; set; }
    public required string Title { get; set; }
    public string? ImageKey { get; set; }
    public string? Description { get; set; }

    public List<RatingEntry> Ratings { get; set; } = [];
}