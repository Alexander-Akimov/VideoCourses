﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Common.AutoMapper
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<Video, VideoDTO>()
                .ForMember(d => d.Module, a => a.MapFrom(c => c.Module.Title))
                .ForMember(d => d.Course, a => a.MapFrom(c => c.Course.Title))
                .ReverseMap()
                .ForMember(d => d.Module, a => a.Ignore())
                .ForMember(d => d.Course, a => a.Ignore());

            CreateMap<Download, DownloadDTO>()
                .ForMember(d => d.Module, a => a.MapFrom(c => c.Module.Title))
                .ForMember(d => d.Course, a => a.MapFrom(c => c.Course.Title))
                .ReverseMap()
                .ForMember(d => d.Module, a => a.Ignore())
                .ForMember(d => d.Course, a => a.Ignore());

            CreateMap<Module, ModuleDTO>()
                .ForMember(d => d.Course, a => a.MapFrom(c => c.Course.Title))
                .ReverseMap()
                .ForMember(d => d.Course, a => a.Ignore())
                .ForMember(d => d.Videos, a => a.Ignore())
                .ForMember(d => d.Downloads, a => a.Ignore());

            CreateMap<Course, CourseDTO>()
                .ForMember(d => d.Instructor, a => a.MapFrom(c => c.Instructor.Name))
                .ReverseMap()                
                .ForMember(d => d.Instructor, a => a.Ignore());

            CreateMap<Instructor, InstructorDTO>().ReverseMap();
        }
    }
}
