using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcProtoLib.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using VOD.Common.Constants;
using VOD.Common.Services;

namespace GrpcGreeter.Services
{
    [Authorize(Policy = nameof(Roles.Admin))]
    public class CoursesService : Courses.CoursesBase
    {
        private readonly ILogger<CoursesService> _logger;

        public CoursesService(ILogger<CoursesService> logger)
        {
            _logger = logger;
        }

        public override Task<CourseMessage> CreateCourse(CourseMessage request, ServerCallContext context)
        {
            CourseMessage result = null;
            try
            {
                result = new CourseMessage()
                {
                    Title = "Smart Title"
                };
            }
            catch (Exception ex)
            {
                //throw;
                _logger.Log(LogLevel.Error, ex.Message);
            }
            return Task.FromResult(result);
        }
    }
}
