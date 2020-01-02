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

            CreateMap<CourseMessage, CourseDTO>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.Id))
                .ForMember(d => d.InstructorId, s => s.MapFrom(p => p.InstructorId)) 
                .ForMember(d => d.Title, s => s.MapFrom(p => p.Title))
                .ForMember(d => d.ImageUrl, s => s.MapFrom(p => p.ImageUrl))
                .ForMember(d => d.MarqueeImageUrl, s => s.MapFrom(p => p.MarqueeImageUrl))
                .ForMember(d => d.Description, s => s.MapFrom(p => p.Description))
                 .ReverseMap();

            CreateMap<TokenDTO, TokenMessage>()
                .ForMember(d => d.Token, a => a.MapFrom(c => c.Token))
                .ForMember(d => d.TokenExpires, a => a.MapFrom(c => Timestamp.FromDateTime(c.TokenExpires)))
                .ReverseMap()
                .ForMember(d => d.TokenExpires, a => a.MapFrom(c => c.TokenExpires.ToDateTime()));
        }
    }
}
