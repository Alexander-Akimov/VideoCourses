using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.UI;
using VOD.Domain.Entities;

namespace VOD.Common.AutoMapper
{
    public class EntityToDTOMappingProfile : Profile
    {
        public EntityToDTOMappingProfile()
        {
            CreateMap<Video, VideoDTO>();

            CreateMap<Download, DownloadDTO>()
                .ForMember(dest => dest.DownloadUrl, opt => opt.MapFrom(s => s.Url))
                .ForMember(dest => dest.DownloadTitle, opt => opt.MapFrom(s => s.Title));

            CreateMap<Instructor, InstructorDTO>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.InstructorDescription, opt => opt.MapFrom(s => s.Description))
                .ForMember(dest => dest.InstructorAvatar, opt => opt.MapFrom(s => s.Thumbnail));

            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(s => s.Title))
                .ForMember(dest => dest.CourseDescription, opt => opt.MapFrom(s => s.Description))
                .ForMember(dest => dest.MarqueeImageUrl, opt => opt.MapFrom(s => s.MarqueeImageUrl))
                .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(s => s.ImageUrl));

            CreateMap<Module, ModuleDTO>()
                            .ForMember(dest => dest.ModuleTitle, opt => opt.MapFrom(s => s.Title))
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
        }

    }
}
