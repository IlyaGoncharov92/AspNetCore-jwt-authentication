using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthJWT.Models;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWT.Providers
{
    public class OAuthProvider
    {
        private IConfiguration Config { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserRepository UserRepository { get; }
        private RefreshTokenRepository RefreshTokenRepository { get; }

        private static TimeSpan AccessTokenExpires => TimeSpan.FromSeconds(15);
        private static DateTime RefreshTokenExpires => DateTime.UtcNow.AddMinutes(2);

        public OAuthProvider(IConfiguration config, IHttpContextAccessor httpContextAccessor, UserRepository userRepository, RefreshTokenRepository refreshTokenRepository)
        {
            Config = config;
            HttpContextAccessor = httpContextAccessor;
            UserRepository = userRepository;
            RefreshTokenRepository = refreshTokenRepository;
        }

        public ResponseData DoPassword(JWTRequest parameters)
        {
            var user = UserRepository.Find(parameters.username, parameters.password);

            if (user == null)
            {
                return new ResponseData
                {
                    Message = "invalid user infomation"
                };
            }

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString("n"),
                ExpiresUtc = RefreshTokenExpires
            };

            RefreshTokenRepository.Add(refreshToken);

            return new ResponseData
            {
                Message = "OK",
                Data = GetJwt(user, refreshToken.Id)
            };
        }

        public ResponseData DoRefreshToken(JWTRequest parameters)
        {
            var token = RefreshTokenRepository.Get(parameters.refresh_token);
            
            var user = UserRepository.Get(parameters.username);

            if (token == null || user == null)
            {
                return new ResponseData
                {
                    Message = "can not refresh token",
                };
            }
            else
            {
                return new ResponseData
                {
                    Message = "OK",
                    Data = GetJwt(user, parameters.refresh_token)
                };
            }
        }

        private JWTResponse GetJwt(User user, string refreshToken)
        {
            var now = DateTime.UtcNow;

            var claims = CreateClaims(user);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config["Jwt:Secret"]));

            var jwt = new JwtSecurityToken(
                issuer: Config["Jwt:Issuer"],
                audience: Config["Jwt:Audience"],
                claims: claims,
                notBefore: now,
                expires: now.Add(AccessTokenExpires),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JWTResponse
            {
                access_token = encodedJwt,
                expires_in = (int) AccessTokenExpires.TotalSeconds,
                refresh_token = refreshToken,
                username = user.Email
            };
        }

        private static List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return claims;
        }
    }
}
