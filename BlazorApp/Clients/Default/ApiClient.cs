
namespace BlazorApp.Clients.Default
{
    public class ApiClient : 
        IApiClient,
        IPokemonApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<TResponse> GetAsync<TResponse>(string requestUri)
            where TResponse : class
        {

            // Send a GET request and get the response
            var response = await _client.GetAsync(requestUri);
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                // Deserialize the content to the generic type TResponse
                var result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(content);
                // Return the result
                return result;
            }
            else
            {
                // Throw an exception with the status code and reason phrase
                throw new HttpRequestException(response.StatusCode + ": " + response.ReasonPhrase);
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string requestUri)
            where TResponse : class
        {
            // Send a DELETE request and get the response
            var response = await _client.DeleteAsync(requestUri);
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                // Deserialize the content to the generic type TResponse
                var result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(content);
                // Return the result
                return result;
            }
            else
            {
                // Throw an exception with the status code and reason phrase
                throw new HttpRequestException(response.StatusCode + ": " + response.ReasonPhrase);
            }
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TResponse : class
            where TRequest : class
        {

            // Convert the content object to JSON string
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            // Create a StringContent object with the JSON string and content type header
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            // Send a POST request and get the response
            var response = await _client.PostAsync(requestUri, httpContent);
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                // Deserialize the content to the generic type TResponse
                var result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(content);
                // Return the result
                return result;
            }
            else
            {
                // Throw an exception with the status code and reason phrase
                throw new HttpRequestException(response.StatusCode + ": " + response.ReasonPhrase);
            }
        }

        public async Task<TResponse> PutAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TResponse : class
            where TRequest : class
        {

            // Convert the content object to JSON string
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            // Create a StringContent object with the JSON string and content type header
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            // Send a PUT request and get the response
            var response = await _client.PutAsync(requestUri, httpContent);
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                // Deserialize the content to the generic type TResponse
                var result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(content);
                // Return the result
                return result;
            }
            else
            {
                // Throw an exception with the status code and reason phrase
                throw new HttpRequestException(response.StatusCode + ": " + response.ReasonPhrase);
            }
        }
    }
}
