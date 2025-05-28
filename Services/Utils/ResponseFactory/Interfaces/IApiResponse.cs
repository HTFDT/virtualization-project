using Microsoft.AspNetCore.Mvc;

namespace Services.Utils.ResponseFactory.Interfaces;

public interface IApiResponse
{
    IActionResult ToActionResult();
}