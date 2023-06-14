using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class BaseSpecification<T> :ISpecification<T> where T : BaseEntity
  
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }= new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
          this.Criteria=criteria;
        }

        public void AddInclude(Expression<Func<T,object>>includeexpression)

        {
            Includes.Add(includeexpression);    
        }

        public void AddOrderBy(Expression<Func<T, object>> orderbyExpression)
        {
            OrderByDescending = orderbyExpression;
        }
        public void AddOrderByDescending(Expression<Func<T, object>> orderbyDesExpression)
        {
            OrderByDescending = orderbyDesExpression;
        }
        public void ApplyPagination(int skip ,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;

        }

    }
}
