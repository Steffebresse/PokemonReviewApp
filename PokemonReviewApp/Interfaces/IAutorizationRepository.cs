using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IAutorizationRepository
    {
        bool CreateUser(User user);
        bool Save();
        (bool,User) UserExists(string userName);

    }
}
