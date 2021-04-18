using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositories;
using Entities.Results;
using Entities.Options;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public IdentityRepository(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return new AuthenticationResult
                {
                    Succeeded = false,
                    Errors = new[] { "User does not exist!" }
                };

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return new AuthenticationResult
                {
                    Succeeded = false,
                    Errors = new[] { "User or Password is wrong!!" }
                };

            return GenerateAuthenticationResult(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string userName, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
                return new AuthenticationResult
                {
                    Succeeded = false,
                    Errors = new[] { "This email address already registered!!" }
                };

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = userName
            };

            var creationResult = await _userManager.CreateAsync(newUser, password);

            if (!creationResult.Succeeded)
                return new AuthenticationResult
                {
                    Succeeded = false,
                    Errors = creationResult.Errors.Select(e => e.Description)
                };

            return GenerateAuthenticationResult(newUser);
        }

        private AuthenticationResult GenerateAuthenticationResult(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Value.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("user_id", newUser.Id)
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                ExpiredIn = int.Parse(_jwtSettings.Value.ExpiredIn),
                Succeeded = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}