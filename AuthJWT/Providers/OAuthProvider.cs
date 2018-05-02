using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Repositories;
using AuthJWT.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWT.Providers
{
    public class OAuthProvider
    {
        private IConfiguration Config { get; }
        private UserRepository UserRepository { get; }
        private RefreshTokenRepository RefreshTokenRepository { get; }

        private static TimeSpan AccessTokenExpires => TimeSpan.FromSeconds(30);
        private static DateTime RefreshTokenExpires => DateTime.UtcNow.AddMinutes(2);

        public OAuthProvider(IConfiguration config, UserRepository userRepository,
            RefreshTokenRepository refreshTokenRepository)
        {
            Config = config;
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
                Data = GetJwt(refreshToken.Id)
            };
        }

        public ResponseData DoRefreshToken(JWTRequest parameters)
        {
            var token = RefreshTokenRepository.Get(parameters.refresh_token);

            if (token == null)
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
                    Data = GetJwt(parameters.refresh_token)
                };
            }
        }

        private JWTResponse GetJwt(string refresh_token)
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var symmetricKeyAsBase64 = Config["Jwt:Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

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
                refresh_token = refresh_token,
            };
        }
    }
}