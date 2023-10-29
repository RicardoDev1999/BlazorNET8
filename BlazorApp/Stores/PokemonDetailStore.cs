using BlazorApp.Models.ViewModels;
using BlazorApp.Services.Interfaces;

namespace BlazorApp.Stores
{
    public class PokemonDetailStore : BaseStore
    {
        private readonly IPokemonService _pokemonService;

        public bool Loading { get; set; } = false;

        private PokemonViewModel _pokemon = null!;
        public PokemonViewModel Pokemon
        {
            get
            {
                return _pokemon;
            }
            set
            {
                _pokemon = value;
                NotifyPropertyChanged();
            }
        }

        public PokemonDetailStore(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        private void ToggleLoading()
        {
            Loading = !Loading;
            NotifyPropertyChanged();
        }

        public async Task GetPokemon(int id)
        {
            ToggleLoading();
            Pokemon = await _pokemonService.GetPokemon(id);
            ToggleLoading();
        }

        public override void Dispose()
        {
            return;
        }
    }
}
