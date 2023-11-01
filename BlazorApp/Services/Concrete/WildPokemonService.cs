using BlazorApp.Clients;
using BlazorApp.Models.Responses;
using BlazorApp.Models.ViewModels;
using System.ComponentModel;

namespace BlazorApp.Services.Concrete
{
    public sealed class WildPokemonService : INotifyPropertyChanged
    {
        private IPokemonApiClient _pokemonApiClient;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PokemonListResponse Pokemons { get; set; }
        public List<PokemonViewModel> WildPokemons { get; set; }

        public WildPokemonService(IPokemonApiClient pokemonApiClient)
        {
            _pokemonApiClient = pokemonApiClient;

            WildPokemons = new();

            Pokemons = _pokemonApiClient.GetAsync<PokemonListResponse>($"pokemon?limit=1000")
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public void GenerateWildPokemon()
        {
            if (Pokemons == null)
                return;

            Random rnd = new Random();
            var pokeId = rnd.Next(0, 1000);

            var wildPokemon = Pokemons.Results.ElementAtOrDefault(pokeId);

            if (wildPokemon == null)
                return;

            WildPokemons.Add(new PokemonViewModel()
            {
                Id = pokeId + 1,
                Name = wildPokemon.Name
            });
            
            if (PropertyChanged != null)
            {
                PropertyChanged(WildPokemons, new PropertyChangedEventArgs(nameof(WildPokemons)));
            }
        }
    }
}
