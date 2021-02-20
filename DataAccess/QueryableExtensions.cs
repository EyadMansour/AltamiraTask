﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DataAccess
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includeProperties) where T : class, IEntity, new()
        {
            foreach (var includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);
            return queryable;
        }
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params string[] includeProperties) where T : class, IEntity, new()
        {
            foreach (var includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);
            return queryable;
        }
        public static IQueryable<T> FindPaged<T>(this IQueryable<T> query, PagingParameters filter)
        {
            return query.Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit);
        }
    }
}
