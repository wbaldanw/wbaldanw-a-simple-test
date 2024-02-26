using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Contracts.UseCases.Users;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Last.Simple.App.UseCases.Users
{
    public class SignInUC : ISignInUC
    {
        private readonly IUserRepository userRepository;
        private readonly IConfigurationSection jwtConfiguration;

        public SignInUC(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.jwtConfiguration = configuration.GetSection("JWT");
        }

        public async Task<string> SignIn(SignInRequest request)
        {
            var user = await userRepository.GetByUserName(request.UserName);
            var signIn = new SignIn(user);

            if (!signIn.IsAuthenticated(request.UserName, request.Password))
            {
                throw new UnauthorizedAccessException("Invalid password or user");
            }            

            var token = GenerateToken(user);

            return token;
        }

        private string GenerateToken(User user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim("userName", user.UserName.ToString())
                };            

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration["Secret"].ToString()));

            var token = new JwtSecurityToken(
                issuer: jwtConfiguration["ValidIssuer"].ToString(),
                audience: jwtConfiguration["ValidAudience"].ToString(),
                expires: DateTime.Now.AddHours(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
