using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using GrpcProtoLib.Protos;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;

namespace VOD.Grpc.Common.AutoMapper
{
    public class GrpcMappingProfile : Profile
    {
        public GrpcMappingProfile()
        {
            CreateMap<UserMessage, LoginUserDTO>()
                .ForMember(d => d.Email, a => a.MapFrom(c => c.Email))
                .ForMember(d => d.Password, a => a.MapFrom(c => c.Password))
                .ForMember(d => d.PasswordHash, a => a.MapFrom(c => c.PasswordHash))
                .ReverseMap()
                .ForMember(d => d.UserId, a => a.Ignore());

            CreateMap<TokenDTO, TokenMessage>()
                .ForMember(d => d.Token, a => a.MapFrom(c => c.Token))
                .ForMember(d => d.TokenExpires, a => a.MapFrom(c => Timestamp.FromDateTime(c.TokenExpires)))
                .ReverseMap()
                .ForMember(d => d.TokenExpires, a => a.MapFrom(c => c.TokenExpires.ToDateTime()));
        }
    }
}
