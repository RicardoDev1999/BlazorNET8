using BlazorApp.Models.Responses;
using System.Text.Json.Serialization;

namespace BlazorApp.Models.ViewModels
{
    public sealed class PokemonListViewModel
    {
        public int Count { get; set; }

        public IEnumerable<PokemonListRowViewModel> Results { get; set; } = Enumerable.Empty<PokemonListRowViewModel>();
    }

    public sealed class PokemonListRowViewModel
    {
        public int Id { get; set; }
        public string FrontSprite { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Url { get; set; }
    }
}
