namespace Services.Utils.ResponseFactory.Interfaces;

public interface IJsonApiResponseBuilder : IApiResponseBuilder<JsonApiResponse>;

public interface IJsonApiResponseBuilder<TModel> : IApiResponseBuilder<IJsonApiResponseModelBuilder<TModel>, JsonApiResponse<TModel>>;