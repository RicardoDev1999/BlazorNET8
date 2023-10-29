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
        private IPokemonService _pokemonService;
        private IFavoritePokemonService _favoritePokemonService;

        public bool Loading { get; set; } = true;
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

        public PokemonListStore(IPokemonService pokemonService, IFavoritePokemonService favoritePokemonService)
        {
            PropertyChanged += PokemonListStore_PropertyChanged;

            _pokemonService = pokemonService;
            _favoritePokemonService = favoritePokemonService;
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

            //Update DOM
            pokemon.IsFavorite = !current;
            NotifyPropertyChanged(nameof(PokemonList));

            //Toggle favorite
            var favorite = await _favoritePokemonService.ToggleFavorite(pokemonId);

            //Update DOM with final state in case of failure
            pokemon.IsFavorite = favorite;
            NotifyPropertyChanged(nameof(PokemonList));
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
