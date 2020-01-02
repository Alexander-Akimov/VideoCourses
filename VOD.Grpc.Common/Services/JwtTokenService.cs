using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GrpcProtoLib.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VOD.Common.Constants;
using VOD.Common.Extensions;
using VOD.Common.Services;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;

namespace VOD.Grpc.Common.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly Tokenizer.TokenizerClient _tokenGrpcClient;
        private readonly IMapper _mapper;
        private readonly ILogger<JwtTokenService> _logger;
        private readonly UserManager<VODUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(UserManager<VODUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            Tokenizer.TokenizerClient tokenGrpcClient,
            IMapper mapper, 
            ILogger<JwtTokenService> logger)
        {
            //_http = http;
            _tokenGrpcClient = tokenGrpcClient;
            _mapper = mapper;
            _logger = logger;
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
                var tokenUser = new UserMessage
                {
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };
                var tokenMessage = await _tokenGrpcClient.CreateTokenAsync(tokenUser);

                var token = _mapper.Map<TokenDTO>(tokenMessage);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                var tokenUser = new UserMessage
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };
                var tokenMessage = await _tokenGrpcClient.GetTokenAsync(tokenUser);

                var newToken = _mapper.Map<TokenDTO>(tokenMessage);
                return newToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default;
            }
        }
    }
}
