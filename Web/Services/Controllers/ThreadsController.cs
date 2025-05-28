using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Types.Threads;

namespace Web.Services.Controllers;

[ApiController]
[Route("api/threads")]
public class ThreadsController(IThreadsService threadsService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateThread([FromForm] CreateThreadRequestDto request) =>
        (await threadsService.CreateThreadAsync(request)).ToActionResult();
    
    [HttpGet]
    [ProducesResponseType<List<ThreadItemDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThreads() =>
        (await threadsService.GetThreadsAsync()).ToActionResult();
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<ThreadResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThread(int id) =>
        (await threadsService.GetThreadAsync(id)).ToActionResult();
    
    [HttpPatch("{id:int}")]
    [ProducesResponseType<ThreadResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditThread(int id, [FromForm] EditThreadRequestDto request) =>
        (await threadsService.EditThreadAsync(id, request)).ToActionResult();
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType<ThreadResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteThread(int id) =>
        (await threadsService.DeleteThreadAsync(id)).ToActionResult();
}