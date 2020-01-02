using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Interfaces.Services;

namespace VOD.Grpc.Common.Services
{
    public class InstructorsAdminService : IGenericAdminService<InstructorDTO>
    {
        public Task<int> CreateAsync(InstructorDTO item)
        {
            throw new NotImplementedException();
        }

        public Task<List<InstructorDTO>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
