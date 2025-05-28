using Domain.Interfaces;

namespace Domain.Types;

public class RatingEntry : IEntity<int>
{
    public int PKey { get; set; }
    public required double Rating { get; set; }
    public string? ImageKey { get; set; }
    public string? Comment { get; set; }

    public int ThreadId { get; set; }
    public ThreadEntity Thread { get; set; } = null!;
}