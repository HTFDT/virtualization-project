using Domain.Types;
using Infrastructure.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Types.Threads;
using Services.Utils;
using Services.Utils.ResponseFactory;

namespace Services.Services;

public class DefaultThreadsService(AppDbContext dbContext, IFileManager fileManager) : IThreadsService
{
    public async Task<JsonApiResponse<int>> CreateThreadAsync(CreateThreadRequestDto request)
    {
        string? imageId = null;
        if (request.Image is not null)
        {
            var guid = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(request.Image.FileName);
            imageId = $"{guid}{ext}";
            await using var s = request.Image.OpenReadStream();
            await fileManager.SaveFileAsync(imageId, s);
        }

        var thread = new ThreadEntity
        {
            Title = request.Title,
            Description = request.Description,
            ImageKey = imageId
        };
        
        await dbContext.AddAsync(thread);
        await dbContext.SaveChangesAsync();

        return ApiResponseFactory.Json<int>(o => o.Success()
            .Model(thread.PKey));
    }

    public async Task<JsonApiResponse> EditThreadAsync(int threadId, EditThreadRequestDto request)
    {
        var thread = await dbContext.Threads.FirstOrDefaultAsync(t => t.PKey == threadId);
        if (thread is null)
            return ApiResponseFactory.Json(o => o.Error(StatusCodes.Status404NotFound, "Thread not found."));
        
        thread.Title = request.Title;
        thread.Description = request.Description;
        var imageId = thread.ImageKey;
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
        
        thread.ImageKey = imageId;
        await dbContext.SaveChangesAsync();

        return ApiResponseFactory.Json(o => o.Success());
    }

    public async Task<JsonApiResponse> DeleteThreadAsync(int threadId)
    {
        var thread = await dbContext.Threads
            .Include(t => t.Ratings)
            .FirstOrDefaultAsync(t => t.PKey == threadId);
        if (thread is null)
            return ApiResponseFactory.Json(o => o.Error(StatusCodes.Status404NotFound, "Thread not found."));

        dbContext.Remove(thread);
        await dbContext.SaveChangesAsync();

        if (thread.ImageKey is not null)
            await fileManager.DeleteFileAsync(thread.ImageKey);
        foreach (var rating in thread.Ratings.Where(r => r.ImageKey is not null))
            await fileManager.DeleteFileAsync(rating.ImageKey!);
        
        return ApiResponseFactory.Json(o => o.Success());
    }

    public async Task<JsonApiResponse<ThreadResponseDto>> GetThreadAsync(int threadId)
    {
        var thread = await dbContext.Threads
            .Include(t => t.Ratings)
            .FirstOrDefaultAsync(t => t.PKey == threadId);
        if (thread is null)
            return ApiResponseFactory.Json<ThreadResponseDto>(o => o.Error(StatusCodes.Status404NotFound, "Thread not found."));

        var response = new ThreadResponseDto
        {
            Title = thread.Title,
            Description = thread.Description,
            ImageKey = thread.ImageKey,
            Ratings = thread.Ratings.Select(r => new RatingEntryItemDto
                {
                    Id = r.PKey,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ImageKey = r.ImageKey
                })
                .ToList()
        };

        return ApiResponseFactory.Json<ThreadResponseDto>(o => o.Success().Model(response));
    }

    public async Task<JsonApiResponse<List<ThreadItemDto>>> GetThreadsAsync()
    {
        var response = await dbContext.Threads.Select(t => new ThreadItemDto
        {
            Id = t.PKey,
            Title = t.Title,
            ImageKey = t.ImageKey,
        }).ToListAsync();
        
        return ApiResponseFactory.Json<List<ThreadItemDto>>(o =>
        {
            if (response.Count == 0) 
                o.NoContent();
            else 
                o.Success().Model(response);
        });
    }
}