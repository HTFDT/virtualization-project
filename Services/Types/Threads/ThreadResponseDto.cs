namespace Services.Types.Threads;

public class ThreadResponseDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageKey { get; set; } = null!;
    public List<RatingEntryItemDto> Ratings { get; set; } = [];
}