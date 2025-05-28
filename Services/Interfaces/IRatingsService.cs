using Services.Types.Ratings;
using Services.Utils.ResponseFactory;

namespace Services.Interfaces;

public interface IRatingsService
{
    Task<JsonApiResponse<int>> CreateRatingEntryAsync(int threadId, CreateRatingEntryRequestDto request);
    Task<JsonApiResponse> EditRatingEntryAsync(int ratingId, EditRatingEntryRequestDto request);
    Task<JsonApiResponse> DeleteRatingEntryAsync(int ratingId);
}