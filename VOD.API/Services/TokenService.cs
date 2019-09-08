using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VOD.Common.Constants;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;

namespace VOD.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TokenService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await _userService.GetUserAsync(loginUserDTO, true);
                if (user == null)
                    throw new UnauthorizedAccessException();

                var claims = GetClaims(user, true);
                var token = CreateToken(claims);

                var succeded = await AddTokenToUserAsync(user.Id, token);
                if (!succeded)
                    throw new SecurityTokenException("Could not add a token to the user");

                return token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDTO, string userId)
        {
            try
            {
                var user = await _userService.GetUserAsync(loginUserDTO, true);
                if (user == null)
                    throw new UnauthorizedAccessException();
                if(!userId.Equals(user.Id))
                    throw new UnauthorizedAccessException();

                return new TokenDTO(user.Token, user.TokenExpires);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<Claim> GetClaims(VODUser user, bool includeUserClaims)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (includeUserClaims)
                foreach (var claim in user.Claims)
                    if (!claim.Type.Equals(ClaimType.Token) && !claim.Type.Equals(ClaimType.TokenExpires))
                        claims.Add(claim);
            return claims;
        }
        private TokenDTO CreateToken(IList<Claim> claims)
        {
            try
            {
                var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(signingKey),
                    SecurityAlgorithms.HmacSha256Signature);
                var duration = int.Parse(_configuration["Jwt:Duration"]);
                var now = DateTime.UtcNow;

                var jwtToken = new JwtSecurityToken(
                   issuer: "http://your-domain.com",
                   audience: "http://audience-domain.com",
                   notBefore: now,
                   expires: now.AddDays(duration),
                   claims: claims,
                   signingCredentials: credentials);

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtTokenHandler.WriteToken(jwtToken);

                return new TokenDTO(token, jwtToken.ValidTo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<bool> AddTokenToUserAsync(string userId, TokenDTO token)
        {
            var userDTO = await _userService.GetUserAsync(userId);
            userDTO.Token.Token = token.Token;
            userDTO.Token.TokenExpires = token.TokenExpires;

            return await _userService.UpdateUserAsync(userDTO);
        }
    }
}
