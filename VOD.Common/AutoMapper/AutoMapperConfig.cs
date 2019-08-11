using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOD.Common.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntityToDTOMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
