using BlazorApp.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BlazorApp.Services.Concrete
{
    public class FavoritePokemonService : IFavoritePokemonService
    {
        private readonly IDistributedCache _distributedCache;

        public FavoritePokemonService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<bool> ToggleFavorite(int pokemonId)
        {
            var key = $"pokemon_fav_{pokemonId}";

            var result = await _distributedCache.GetStringAsync(key);
            
            if(bool.TryParse(result, out bool isFavorite))
            {
                await _distributedCache.SetStringAsync(key, (!isFavorite).ToString());
                return !isFavorite;
            }

            await _distributedCache.SetStringAsync(key, true.ToString());
            return true;
        }

        public async Task<(int Id, bool IsFavorite)> IsFavorite(int pokemonId)
        {
            var key = $"pokemon_fav_{pokemonId}";

            var result = await _distributedCache.GetStringAsync(key);

            (int Id, bool IsFavorite) ret = new() { Id = pokemonId, IsFavorite = false };

            if (bool.TryParse(result, out bool isFavorite))
            {
                ret.IsFavorite = isFavorite;
            }

            return ret;
        }
    }
}
