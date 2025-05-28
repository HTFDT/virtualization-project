using Services.Utils.ResponseFactory.Interfaces;

namespace Services.Utils.ResponseFactory;

internal class JsonApiResponseModelBuilder<TModel> : IJsonApiResponseModelBuilder<TModel>
{
    private TModel? _model;
    
    public void Model(TModel model)
    {
        _model = model;
    }

    public JsonApiResponse<TModel> Build()
    {
        return new JsonApiResponse<TModel>
        {
            Model = _model
        };
    }
}