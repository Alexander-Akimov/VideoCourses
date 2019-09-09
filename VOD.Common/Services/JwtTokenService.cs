using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using VOD.Common.Constants;
using VOD.Common.Extensions;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;

namespace VOD.Common.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IHttpClientFactoryService _http;
        private readonly UserManager<VODUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IHttpClientFactoryService http,
            UserManager<VODUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TokenDTO> CheckTokenAsync(TokenDTO token)
        {
            try
            {
                if (token.TokenHasExpired)
                {
                    token = await GetTokenAsync();
                    if (token?.TokenHasExpired ?? true)
                        token = await CreateTokenAsync();
                }
                return token;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<TokenDTO> CreateTokenAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User
                    .FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = await _userManager.FindByIdAsync(userId);
                var tokenUser = new LoginUserDTO
                {
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };
                var token = await _http.CreateTokenAsync(tokenUser, "api/token", AppConstants.HttpClientName);

                return token;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<TokenDTO> GetTokenAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User
                   .FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = await _userManager.FindByIdAsync(userId);
                var claims = await _userManager.GetClaimsAsync(user);

                var token = claims.SingleOrDefault(c => c.Type.Equals(ClaimType.Token))?.Value;
                var date = claims.SingleOrDefault(c => c.Type.Equals(ClaimType.TokenExpires))?.Value;

                DateTime expires;
                var succeeded = DateTime.TryParse(date, out expires);

                // Return token from the user object
                if (succeeded && !token.IsNullOrEmptyOrWhiteSpace())
                    return new TokenDTO(token, expires);

                // Return token from the API
                var tokenUser = new LoginUserDTO
                {
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };

                var newToken = await _http.GetTokenAsync(tokenUser, $"api/token/{user.Id}", AppConstants.HttpClientName);
                return newToken;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
