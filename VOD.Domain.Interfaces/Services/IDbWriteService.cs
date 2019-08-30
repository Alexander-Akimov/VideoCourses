using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Domain.Interfaces
{
    public interface IDbWriteService
    {
        Task<bool> SaveChangesAsync();
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
    }
}
