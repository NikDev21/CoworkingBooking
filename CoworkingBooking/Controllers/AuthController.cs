using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoworkingBooking.Models.DTO;


namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            //is the user already registered?
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("User already exists");

            // 
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok("Registration succeeded");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid login or password");

            var jwt = GenerateJwt(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(70);
            await context.SaveChangesAsync();

            return Ok(new
            {
                Token = jwt,
                refreshToken
            });
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            //Get the token
            var principal = GetPrincipalFromExpiredToken(tokenDto.Token);
            //Get userId from the token
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            //Get the user from the database
            var user = await context.Users.FindAsync(int.Parse(userId!));

            //Check if the user is null or the refresh token is invalid
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            
            var newJwt = GenerateJwt(user);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(70);
            await context.SaveChangesAsync();
            return Ok(new
            {
                Token = newJwt,
                refreshToken = newRefreshToken
            });
        }

        private string GenerateJwt(User user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Create claims for the token(There are uderId and email in token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            //Create the token and descreibe construction of the token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"]!)),
                signingCredentials: creds);

            //Return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        //This method is used to get the claims from the expired (S)token
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            // Create token validation parameters
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
                ValidateLifetime = false, // we want to get claims from expired token
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"]
            };
            // Create token handler and validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            // Validate the token and get the claims principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            return principal;// Return iformation about user(UserId and email)
        }
    }
}
