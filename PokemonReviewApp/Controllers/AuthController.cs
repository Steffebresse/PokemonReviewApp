using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAutorizationRepository _authorizationRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IAutorizationRepository authorizationRepository, IMapper map, IConfiguration configuration)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = map;
            _configuration = configuration;
        }

        

        [HttpPost("registerAdmin")]
        public ActionResult<User> Register(UserDto request)
        {

            User user = new();

            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            user.Username = request.UserName;
            user.PasswordHash = passwordHash;

            
            _authorizationRepository.CreateUser(user);
            

            return Ok(user);
        }

        [HttpPost("registerUser")]
        public ActionResult<User> RegisterUser(UserDto request)
        {

            User user = new();

            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            user.Username = request.UserName;
            user.PasswordHash = passwordHash;


            _authorizationRepository.CreateUser(user);


            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            // Check if the user exists and retrieve the user and existence status
            var (exists, user) = _authorizationRepository.UserExists(request.UserName);

            // If user does not exist, return unauthorized
            if (!exists)
            {
                return Unauthorized("User not found.");
            }

            // Validate the provided password with the stored hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash);

            // If password is incorrect, return unauthorized
            if (!isPasswordValid)
            {
                return Unauthorized("Incorrect password.");
            }

            // If login is successful, return the user object (or a success message)

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>();

            if(user.Username == "Admin" && BCrypt.Net.BCrypt.Verify("Admin", user.PasswordHash))
            {
                List<Claim> claimsAd = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User")
                };

                claims = claimsAd;  
            }
            else
            {
                List<Claim> claimsUs = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, user.Username),
                
                new Claim(ClaimTypes.Role, "User")
                };

                claims = claimsUs;
            }

            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!)); // Skapar en enkodning baserat på värdet i Appsettings

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }



    }
}
