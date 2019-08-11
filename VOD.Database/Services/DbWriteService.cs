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
            catch (Exception )
            {
                throw; // why do i need this? if so then why do we need try/catch?
            }
        }
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                var entityObj = _dbContext.Find<TEntity>(entity.GetType().GetProperty("Id").GetValue(entity));
                if (entityObj != null)
                    _dbContext.Entry(entityObj).State = //do this to be able to update the entity
                        EntityState.Detached; //found entity isn’t tracked now

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
