using BlazorApp.Models.Responses;
using BlazorApp.Models.ViewModels;

namespace BlazorApp.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonListViewModel> GetPokemonList(int page);
        Task<PokemonViewModel> GetPokemon(int id);
    }
}
