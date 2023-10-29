using System.Text.Json.Serialization;

namespace BlazorApp.Models.Responses
{
    public class PokemonResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public PokemonSpritesResponse Sprites { get; set; }    
    }

    public class PokemonSpritesResponse
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }
    }
}
