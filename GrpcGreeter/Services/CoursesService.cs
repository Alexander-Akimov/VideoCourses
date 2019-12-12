using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcGreeter.Protos;
using Microsoft.Extensions.Logging;

namespace GrpcGreeter.Services
{
    public class CoursesService : Protos.CoursesService.CoursesServiceBase
    {
        private readonly ILogger<CoursesService> _logger;
        public CoursesService(ILogger<CoursesService> logger)
        {
            _logger = logger;
        }

        public override Task<CourseReply> CreateCourse(CourseRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CourseReply()
            {
                Courses = { new CourseRequest { Title = "Smart Title" } }
            });
        }
    }
}
