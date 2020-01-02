using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Domain.Interfaces.Services
{
   public interface IGenericAdminService<EntityType> where EntityType : class
    {
        Task<List<EntityType>> GetAsync();
        Task<int> CreateAsync(EntityType item);
    }
}
