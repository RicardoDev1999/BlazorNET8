using System.Text.Json.Serialization;

namespace BlazorApp.Models.Responses
{
    public sealed class PokemonListResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<PokemonListRow> Results { get; set; } = Enumerable.Empty<PokemonListRow>(); 
    }

    public sealed class PokemonListRow
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
