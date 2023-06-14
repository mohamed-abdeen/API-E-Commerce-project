using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithFilterForCountSpec : BaseSpecification<Product>
    {

        public ProductWithFilterForCountSpec(ProductSpecificationPrams productparams)
          : base(p =>

            (string.IsNullOrEmpty(productparams.search) || p.Name.ToLower().Contains(productparams.search)) &&
          (!productparams.Brandid.HasValue || p.ProductBrandId == productparams.Brandid.Value) &&
          (!productparams.Typeid.HasValue || p.ProductTypeId == productparams.Typeid.Value)
               )
        {

        }
    }
}
