using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using GrpcProtoLib.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VOD.API.Services;
using VOD.Domain.DTOModles.Admin;

namespace GrpcGreeter.Services
{
    public class TokenGrpcService : Tokenizer.TokenizerBase
    {
        private readonly ILogger<TokenGrpcService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public TokenGrpcService(ILogger<TokenGrpcService> logger, ITokenService tokenService, IMapper mapper)
        {
            _logger = logger;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public override async Task<TokenMessage> CreateToken(UserMessage request, ServerCallContext context)
        {
            try
            {
                var loginUserDto = _mapper.Map<LoginUserDTO>(request);

                var jwt = await _tokenService.GenerateTokenAsync(loginUserDto);
                if (jwt.Token == null)
                    return null;

                var tokenMessage = _mapper.Map<TokenMessage>(jwt);                                

                return tokenMessage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override async Task<TokenMessage> GetToken(UserMessage request, ServerCallContext context)
        {
            try
            {
                var loginUserDto = _mapper.Map<LoginUserDTO>(request);

                var jwt = await _tokenService.GetTokenAsync(loginUserDto, request.UserId);
                if (jwt.Token == null)
                    return null;
                var tokenMessage = _mapper.Map<TokenMessage>(jwt);

                return tokenMessage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
