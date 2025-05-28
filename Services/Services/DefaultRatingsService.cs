using Domain.Types;
using Infrastructure.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Types.Ratings;
using Services.Utils;
using Services.Utils.ResponseFactory;

namespace Services.Services;

public class DefaultRatingsService(AppDbContext dbContext, IFileManager fileManager) : IRatingsService
{
    public async Task<JsonApiResponse<int>> CreateRatingEntryAsync(int threadId, CreateRatingEntryRequestDto request)
    {
        var thread = await dbContext.Threads.FirstOrDefaultAsync(r => r.PKey == threadId);
        if (thread is null)
            return ApiResponseFactory.Json<int>(o => o.Error(StatusCodes.Status404NotFound, "Thread not found."));
        
        string? imageId = null;
        if (request.Image is not null)
        {
            var guid = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(request.Image.FileName);
            imageId = $"{guid}{ext}";
            await using var s = request.Image.OpenReadStream();
            await fileManager.SaveFileAsync(imageId, s);
        }

        var ratingEntry = new RatingEntry
        {
            Rating = request.Rating,
            Comment = request.Comment,
            ImageKey = imageId,
            ThreadId = threadId
        };
        
        await dbContext.AddAsync(ratingEntry);
        await dbContext.SaveChangesAsync();

        return ApiResponseFactory.Json<int>(o => o.Success()
            .Model(ratingEntry.PKey));
    }

    public async Task<JsonApiResponse> EditRatingEntryAsync(int ratingId, EditRatingEntryRequestDto request)
    {
        var ratingEntry = await dbContext.Ratings.FirstOrDefaultAsync(r => r.PKey == ratingId);
        if (ratingEntry is null)
            return ApiResponseFactory.Json(o => o.Error(StatusCodes.Status404NotFound, "Rating entry not found."));
        
        ratingEntry.Rating = request.Rating;
        ratingEntry.Comment = request.Comment;
        var imageId = ratingEntry.ImageKey;
        if (request.Image is not null)
        {
            var prevImageId = imageId;
            
            var guid = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(request.Image.FileName);
            imageId = $"{guid}{ext}";
            await using var s = request.Image.OpenReadStream();
            await fileManager.SaveFileAsync(imageId, s);
            
            if (prevImageId is not null)
                await fileManager.DeleteFileAsync(prevImageId);
        }
        
        ratingEntry.ImageKey = imageId;
        await dbContext.SaveChangesAsync();

        return ApiResponseFactory.Json(o => o.Success());
    }

    public async Task<JsonApiResponse> DeleteRatingEntryAsync(int ratingId)
    {
        var thread = await dbContext.Ratings.FirstOrDefaultAsync(r => r.PKey == ratingId);
        if (thread is null)
            return ApiResponseFactory.Json(o => o.Error(StatusCodes.Status404NotFound, "Rating entry not found."));

        dbContext.Remove(thread);
        await dbContext.SaveChangesAsync();

        if (thread.ImageKey is not null)
            await fileManager.DeleteFileAsync(thread.ImageKey);
        
        return ApiResponseFactory.Json(o => o.Success());
    }
}