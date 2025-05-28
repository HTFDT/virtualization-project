namespace Services.Types.Shared;

public class JsonResponseDto
{
    public int Status { get; set; }
    public string? Error { get; set; }
}

public class JsonResponseDto<TResponse> : JsonResponseDto
{
    public TResponse? Response { get; set; }
}