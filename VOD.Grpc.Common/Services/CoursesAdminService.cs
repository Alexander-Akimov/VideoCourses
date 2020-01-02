using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcProtoLib.Protos;
using VOD.Common.Services;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Interfaces.Services;

namespace VOD.Grpc.Common.Services
{
    public class CoursesAdminService : IGenericAdminService<CourseDTO>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly Courses.CoursesClient _coursesClient;
        private readonly IMapper _mapper;
        private TokenDTO _token = new TokenDTO();

        public CoursesAdminService(IJwtTokenService jwtTokenService,
            Courses.CoursesClient coursesClient, IMapper mapper)
        {
            _jwtTokenService = jwtTokenService;
            _coursesClient = coursesClient;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CourseDTO item)
        {
            _token = await _jwtTokenService.CheckTokenAsync(_token);

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_token.Token}");

            var course = _mapper.Map<CourseMessage>(item);//new CourseMessage { Title = "BestTitle" }

            var reply = await _coursesClient.CreateCourseAsync(course, headers);

            return reply.Id;
        }

        public async Task<List<CourseDTO>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
