using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {

        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon CreatePokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var pokemonCategoryEntity = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = CreatePokemon,

            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = pokemonCategoryEntity,
                Pokemon = CreatePokemon,
            };

            _context.Add(pokemonOwner);
            _context.Add(pokemonCategory);
            _context.Add(CreatePokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int pokemonId)
        {
            return _context.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokemonId);

            if (review.Count() <= 0)
            return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokemonId)
        {
            return _context.Pokemon.Any(p => p.Id == pokemonId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon UpdatedPokemon)
        {
            _context.Update(UpdatedPokemon);
            return Save();
        }
    }
}
