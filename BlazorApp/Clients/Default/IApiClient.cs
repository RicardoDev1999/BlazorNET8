namespace BlazorApp.Clients.Default
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(string requestUri)
            where TResponse : class;
        Task<TResponse> PostAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TRequest : class
            where TResponse : class;
        Task<TResponse> PutAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TRequest : class
            where TResponse : class;
        Task<TResponse> DeleteAsync<TResponse>(string requestUri)
            where TResponse : class;
    }
}
