using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Database.Services
{
    public class DbWriteService : IDbWriteService
    {
        private VODContext _dbContext;
        public DbWriteService(VODContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Remove(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                var entityObj = _dbContext.Find<TEntity>(entity.GetType().GetProperty("id").GetValue(entity));
                if (entityObj != null)
                    _dbContext.Entry(entityObj).State = //do this to be able to update the entity
                        EntityState.Detached; //The entity isn’t tracked

                _dbContext.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
