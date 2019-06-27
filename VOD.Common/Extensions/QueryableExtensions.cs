using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOD.Common.Extensions
{    
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> items, IEnumerable<string> propertyNames) where TEntity : class
        {
            foreach (var name in propertyNames)
                items = items.Include(name);

            return items;
        }
    }
}
