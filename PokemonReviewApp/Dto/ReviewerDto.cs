using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class ReviewerDto
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

       // public ICollection<Review> Reviews { get; set; } // <------ Denna om du inte fattar UPDATE: tog bort denna då det sket sig när jag skulle postea

    }
}
