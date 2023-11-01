using BlazorApp.Clients;
using BlazorApp.Models.Responses;
using BlazorApp.Models.ViewModels;
using BlazorApp.Services.Interfaces;

namespace BlazorApp.Services.Concrete
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly IFavoritePokemonService _favoritePokemonService;
        private readonly ILogger<PokemonService> _logger;

        public const int LIST_LIMIT = 10;

        public PokemonService(IPokemonApiClient pokemonApiClient, IFavoritePokemonService favoritePokemonService, ILogger<PokemonService> logger)
        {
            _pokemonApiClient = pokemonApiClient;
            _favoritePokemonService = favoritePokemonService;
            _logger = logger;
        }

        public async Task<PokemonListViewModel> GetPokemonList(int page)
        {
            var offset = (page - 1) * LIST_LIMIT;
            var pokemonList = await _pokemonApiClient.GetAsync<PokemonListResponse>($"pokemon?offset={offset}&limit={LIST_LIMIT}");

            if(pokemonList == null || pokemonList.Count == 0)
            {
                return new PokemonListViewModel();
            }

            var pokemonDetailsTasks = pokemonList.Results
                .Select(x => _pokemonApiClient.GetAsync<PokemonResponse>($"pokemon/{x.Name}"))
                .ToArray();

            await Task.WhenAll(pokemonDetailsTasks);

            var pokemonIsFavoriteTasks = pokemonDetailsTasks
                .Select(x => x.Result)
                .Select(x => _favoritePokemonService.IsFavorite(x.Id))
                .ToArray();

            try 
            {
                await Task.WhenAll(pokemonIsFavoriteTasks);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            var ret = new PokemonListViewModel()
            {
                Count = pokemonList.Count,
                Results = pokemonList.Results
                    .Select(x => {
                        var pokeDetail = pokemonDetailsTasks
                            .FirstOrDefault(pd => pd.Result.Name == x.Name)
                            ?.Result;

                        var pokeFavorite = pokemonIsFavoriteTasks
                            ?.FirstOrDefault(pd => pd.IsCompletedSuccessfully && pd.Result.Id == pokeDetail.Id)
                            ?.Result;

                        return new
                        {
                            pokeDetail?.Id,
                            pokeDetail?.Sprites?.FrontDefault,
                            x.Name,
                            x.Url,
                            IsFavorite = pokeFavorite.HasValue 
                                ? pokeFavorite.Value.IsFavorite
                                : false
                        };
                    })
                    .Select(x => new PokemonListRowViewModel()
                    {
                        Id = x.Id.Value,
                        FrontSprite = x.FrontDefault,
                        Name = x.Name,
                        Url = x.Url,
                        IsFavorite = x.IsFavorite
                    }).ToList()
            };

            return ret;
        }

        public async Task<PokemonViewModel> GetPokemon(int id)
        {
            var pokemonResult = await _pokemonApiClient.GetAsync<PokemonResponse>($"pokemon/{id}");

            if (pokemonResult == null)
                return null;

            var isFavoritePokemon = await _favoritePokemonService.IsFavorite(id);

            return new PokemonViewModel()
            {
                Id = pokemonResult.Id,
                Name = pokemonResult.Name,
                FrontSprite = pokemonResult.Sprites.FrontDefault,
                IsFavorite = isFavoritePokemon.IsFavorite
            };
        }
    }
}
