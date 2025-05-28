namespace Services.Types.Threads;

public class ThreadItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? ImageKey { get; set; }
}