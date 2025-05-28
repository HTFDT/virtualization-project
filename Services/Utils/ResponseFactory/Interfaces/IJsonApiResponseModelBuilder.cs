namespace Services.Utils.ResponseFactory.Interfaces;

public interface IJsonApiResponseModelBuilder<TModel> : IBuilder<JsonApiResponse<TModel>>
{
    void Model(TModel model);
}