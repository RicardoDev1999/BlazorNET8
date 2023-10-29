namespace BlazorApp.Services.Interfaces
{
    public interface IFavoritePokemonService
    {
        Task<bool> ToggleFavorite(int pokemonId);
        Task<(int Id, bool IsFavorite)> IsFavorite(int pokemonId);
    }
}
