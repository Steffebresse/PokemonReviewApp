using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{

    // Anledningen till att vi använder Repos är för att separera hätmningen från databasen och hanteringen av http requesten (404 o skit)
    // Samt om vi kör allt direkt i kontrollen blir det mycket logik i en o samma kontroll, så det är bättre att dela upp det.
    // Samt om man delar upp det kan man lättare testa sin datahätmning med mocks osv.
    // Samt genom att använda hätmningen direkt i kontrollern kan man repetera mycket kod, typ, getcategories måste du bygga upp flera gånger i olika 
    // requests då den aldrig blir färdig-definerad.

    public class AuthorizationRepository : IAutorizationRepository
    {

        private readonly DataContext _context;

        public AuthorizationRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);

            return Save();


        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public (bool, User) UserExists(string userName)
        {
            
            User foundUser = _context.Users.FirstOrDefault(un => un.Username == userName);

            
            bool exists = foundUser != null;

            
            return (exists, foundUser);
        }
    }
}
