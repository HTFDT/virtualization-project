using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Services.Types.Shared;
using Services.Utils.ResponseFactory.Interfaces;

namespace Services.Utils.ResponseFactory;

public class JsonApiResponse : IApiResponse
{
    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    
    public virtual IActionResult ToActionResult()
    {
        return new JsonResult(new JsonResponseDto
        {
            Status = StatusCode,
            Error = Error
        }, JsonSerializerOptions)
        {
            StatusCode = StatusCode
        };
    }
}

public class JsonApiResponse<TModel> : JsonApiResponse
{
    public TModel? Model { get; set; }
    
    public override IActionResult ToActionResult()
    {
        return new JsonResult(new JsonResponseDto<TModel>
        {
            Status = StatusCode,
            Response = Model,
            Error = Error
        }, JsonSerializerOptions)
        {
            StatusCode = StatusCode
        };
    }
}