namespace Ambev.DeveloperEvaluation.WebApi.Common;

public record ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }
}
