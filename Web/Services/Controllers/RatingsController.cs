using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Types.Ratings;
using Services.Types.Threads;

namespace Web.Services.Controllers;

[ApiController]
[Route("api/threads/{threadId:int}/ratings")]
public class RatingsController(IRatingsService ratingsService) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRating(int threadId, [FromForm] CreateRatingEntryRequestDto request) =>
        (await ratingsService.CreateRatingEntryAsync(threadId, request)).ToActionResult();
    
    [HttpPatch("{id:int}")]
    [ProducesResponseType<ThreadResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditRating(int id, [FromForm] EditRatingEntryRequestDto request) =>
        (await ratingsService.EditRatingEntryAsync(id, request)).ToActionResult();
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType<ThreadResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(int id) =>
        (await ratingsService.DeleteRatingEntryAsync(id)).ToActionResult();
}