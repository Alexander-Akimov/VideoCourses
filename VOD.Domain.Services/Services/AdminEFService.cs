using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services
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

        public async Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
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
            catch (Exception ex ) {
                throw ex;
            }
            return -1;
        }

        public async Task<bool> DeleteAsync<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class
        {
            try
            {
                //TODO: TWO Queries to delete entity????
                //_dbWriteService.Delete(new Entity{Id = })
                var entity = await _dbReadService.SingleAsync<TSource>(expression);
                _dbWriteService.Delete(entity);

                return await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            try
            {
                var entity = _mapper.Map<TDestination>(item);
                _dbWriteService.Update(entity);

                return await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false)
            where TSource : class
            where TDestination : class
        {
            /*if (include)
                _dbReadService.Include<TSource>();*/
            var entities = await _dbReadService.GetAsync<TSource>();

            return _mapper.Map<List<TDestination>>(entities);
        }

        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(
            params Expression<Func<TSource, object>>[] navPropPaths)
            where TSource : class
            where TDestination : class
        {
            var entities = await _dbReadService.GetAsync<TSource>(navPropPaths);

            return _mapper.Map<List<TDestination>>(entities);
        }

        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            var entities = await _dbReadService.GetAsync<TSource>(expression, include);
            return _mapper.Map<List<TDestination>>(entities);
        }

        public async Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            var entity = await _dbReadService.SingleAsync<TSource>(expression, include);
            return _mapper.Map<TDestination>(entity);
        }

        public async Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, params Expression<Func<TSource, object>>[] navPropPaths)
            where TSource : class
            where TDestination : class
        {
            var entity = await _dbReadService.SingleAsync<TSource>(expression, navPropPaths);
            return _mapper.Map<TDestination>(entity);
        }

        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, params Expression<Func<TSource, object>>[] include)
            where TSource : class
            where TDestination : class
        {
            var entities = await _dbReadService.GetAsync<TSource>(expression, include);
            return _mapper.Map<List<TDestination>>(entities);
        }
    }
}
