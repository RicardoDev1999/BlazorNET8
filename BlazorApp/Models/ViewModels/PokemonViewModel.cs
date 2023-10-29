namespace BlazorApp.Models.ViewModels
{
    public sealed class PokemonViewModel
    {
        public int Id { get; set; }
        public string FrontSprite { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
    }
}
