namespace PokemonReviewApp.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
    }
}
