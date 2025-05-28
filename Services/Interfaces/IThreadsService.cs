using Services.Types.Threads;
using Services.Utils.ResponseFactory;

namespace Services.Interfaces;

public interface IThreadsService
{
    Task<JsonApiResponse<int>> CreateThreadAsync(CreateThreadRequestDto request);
    Task<JsonApiResponse> EditThreadAsync(int threadId, EditThreadRequestDto request);
    Task<JsonApiResponse> DeleteThreadAsync(int threadId);
    Task<JsonApiResponse<ThreadResponseDto>> GetThreadAsync(int threadId);
    Task<JsonApiResponse<List<ThreadItemDto>>> GetThreadsAsync();
}