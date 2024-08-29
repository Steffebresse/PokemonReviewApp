using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {

        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokemonId);
        bool PokemonExists(int pokemonId);
       
        bool CreatePokemon(int ownerId, int categoryId, Pokemon createPokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon UpdatedPokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();

    }
}
