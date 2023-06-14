using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository.Data
{
    internal class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria !=null)
                query = query.Where(spec.Criteria);
          
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.Criteria);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.Criteria);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);


            query = spec.Includes.Aggregate(query, (currentquery, IncludeExpression) => currentquery.Include(IncludeExpression));

            return query;
        }
    }
}
