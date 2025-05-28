using Services.Exceptions;
using Services.Utils.ResponseFactory.Interfaces;

namespace Services.Utils.ResponseFactory;

internal class JsonApiResponseBuilder : IJsonApiResponseBuilder
{
    private int? _statusCode;
    private string? _error;
    
    public void NoContent()
    {
        _statusCode = 204;
    }

    public void Success()
    {
        _statusCode = 200;
    }

    public void Error(int statusCode, string? message = null)
    {
        _statusCode = statusCode;
        _error = message;
    }

    public JsonApiResponse Build()
    {
        if (_statusCode is null)
            throw new ResponseBuilderException("Cannot build response without status code");
        return new JsonApiResponse
        {
            StatusCode = _statusCode.Value,
            Error = _error
        };
    }
}

internal class JsonApiResponseBuilder<TModel> : IJsonApiResponseBuilder<TModel>
{
    private int? _statusCode;
    private string? _error;
    private IBuilder<JsonApiResponse<TModel>> _child;

    public IJsonApiResponseModelBuilder<TModel> Error(int statusCode, string? message = null)
    {
        _statusCode = statusCode;
        _error = message;
        var modelBuilder = new JsonApiResponseModelBuilder<TModel>();
        _child = modelBuilder;
        return modelBuilder;
    }

    public IJsonApiResponseModelBuilder<TModel> Success()
    {
        _statusCode = 200;
        var modelBuilder = new JsonApiResponseModelBuilder<TModel>();
        _child = modelBuilder;
        return modelBuilder;
    }

    public IJsonApiResponseModelBuilder<TModel> NoContent()
    {
        _statusCode = 204;
        var modelBuilder = new JsonApiResponseModelBuilder<TModel>();
        _child = modelBuilder;
        return modelBuilder;
    }
    
    public JsonApiResponse<TModel> Build()
    {
        if (_statusCode is null || _child is null)
            throw new ResponseBuilderException("Cannot build response without status code");
        var response = _child.Build();
        response.Error = _error;
        response.StatusCode = _statusCode.Value;
        return response;
    }
}