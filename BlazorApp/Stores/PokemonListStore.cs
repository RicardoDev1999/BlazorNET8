using BlazorApp.Clients;
using BlazorApp.Models.Responses;
using BlazorApp.Models.ViewModels;
using BlazorApp.Services.Concrete;
using BlazorApp.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorApp.Stores
{
    public class PokemonListStore : BaseStore
    {
        private readonly IPokemonService _pokemonService;
        private readonly IFavoritePokemonService _favoritePokemonService;
        private readonly ILogger<PokemonListStore> _logger;

        public bool Loading { get; set; } = true;

        public List<int> SpecificLoading { get; set; } = [];

        public int MaxPage { get { return _pokemonListResponse.Count / PokemonService.LIST_LIMIT; } }

        private int _page = 1;
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                NotifyPropertyChanged();
            }
        }

        private PokemonListViewModel _pokemonListResponse = new();
        public PokemonListViewModel PokemonList
        {
            get
            {
                return _pokemonListResponse;
            }
            set
            {
                _pokemonListResponse = value;
                NotifyPropertyChanged();
            }
        }

        public PokemonListStore(IPokemonService pokemonService, IFavoritePokemonService favoritePokemonService, ILogger<PokemonListStore> logger)
        {
            PropertyChanged += PokemonListStore_PropertyChanged;

            _pokemonService = pokemonService;
            _favoritePokemonService = favoritePokemonService;
            _logger = logger;
        }

        private void ToggleLoading(bool loading)
        {
            Loading = loading;
            NotifyPropertyChanged();
        }

        public async Task GetAsync(int page)
        {
            ToggleLoading(true);
            PokemonList = await _pokemonService.GetPokemonList(page);
            ToggleLoading(false);
        }

        public async Task SetFavorite(int pokemonId, bool current)
        {
            var pokemon = PokemonList.Results.FirstOrDefault(x => x.Id == pokemonId);

            //Toggle Loading
            SpecificLoading.Add(pokemonId);
            NotifyPropertyChanged(nameof(SpecificLoading));

            //Toggle favorite
            try
            {
                var favorite = await _favoritePokemonService.ToggleFavorite(pokemonId);
                await Task.Delay(500);
                pokemon.IsFavorite = favorite;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            //Toggle Loading
            SpecificLoading.Remove(SpecificLoading.FirstOrDefault(x => x == pokemonId));
            NotifyPropertyChanged(nameof(SpecificLoading));
        }

        private async void PokemonListStore_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Page))
            {
                await GetAsync(Page);
            }
        }

        public override void Dispose()
        {
            PropertyChanged -= PokemonListStore_PropertyChanged;
        }
    }
}
