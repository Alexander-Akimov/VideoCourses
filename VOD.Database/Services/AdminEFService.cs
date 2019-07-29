using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Services;

namespace VOD.Database.Services
{
    public class AdminEFService : IAdminService
    {
        private readonly IDbReadService _dbReadService;
        private readonly IDbWriteService _dbWriteService;
        private readonly IMapper _mapper;

        public AdminEFService(IDbReadService dbReadService, IDbWriteService dbWriteService, IMapper mapper)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _dbReadService.AnyAsync(expression);
        }

        public async Task<int> CreateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            try
            {
                var entity = _mapper.Map<TDestination>(item);
                _dbWriteService.Add(entity);

                var succeeded = await _dbWriteService.SaveChangesAsync();
                if (succeeded)
                    return (int)entity.GetType().GetProperty("Id").GetValue(entity);
            }
            catch (Exception) { }
            return -1;
        }

        public async Task<bool> DeleteAsync<TSourse>(Expression<Func<TSourse, bool>> expression) where TSourse : class
        {
            try
            {
                //TODO: TWO Queries to delete entity????
                //_dbWriteService.Delete(new Entity{Id = })
                var entity = await _dbReadService.SingleAsync<TSourse>(expression);
                _dbWriteService.Delete(entity);

                return await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            try
            {
                var entity = _mapper.Map<TDestination>(item);
                _dbWriteService.Update(entity);

                return await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception) { }
            return false;
        }

        public async Task<List<TDestination>> GetAsync<TSourse, TDestination>(bool include = false)
            where TSourse : class
            where TDestination : class
        {
            /*if (include)
                _dbReadService.Include<TSourse>();*/
            var entities = await _dbReadService.GetAsync<TSourse>();
            return _mapper.Map<List<TDestination>>(entities);
        }

        public async Task<List<TDestination>> GetAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            var entities = await _dbReadService.GetAsync<TSourse>(expression);
            return _mapper.Map<List<TDestination>>(entities);
        }

        public async Task<TDestination> SingleAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            var entity = await _dbReadService.SingleAsync<TSourse>(expression);
            return _mapper.Map<TDestination>(entity);
        }
    }
}
