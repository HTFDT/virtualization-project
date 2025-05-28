namespace Services.Types.Threads;

public class RatingEntryItemDto
{
    public int Id { get; set; }
    public double Rating { get; set; }
    public string? Comment { get; set; }
    public string? ImageKey { get; set; }
}