using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandAndTypeSpwcifications : BaseSpecification<Product>
    {
        // Get All product
        public ProductWithBrandAndTypeSpwcifications(ProductSpecificationPrams productparams)
            : base(p =>
            (string.IsNullOrEmpty(productparams.search)||p.Name.ToLower().Contains(productparams.search))&&
            (!productparams.Brandid.HasValue || p.ProductBrandId == productparams.Brandid.Value) &&
            (!productparams.Typeid.HasValue || p.ProductTypeId == productparams.Typeid.Value)
                 )
        {
            AddInclude(p => p.productBrand);
            AddInclude(p => p.productType);

            ApplyPagination(productparams.pageSize *(productparams.PageIndex - 1), productparams.pageSize);



            AddInclude(p => p.Name);
            if (!string.IsNullOrEmpty(productparams.Sort))
            {
                switch (productparams.Sort)
                {
                    case "priceAsc":
                        AddInclude(p => p.Price);
                        break;

                    case "priceDesc":
                        AddInclude(p => p.Price);
                        break;
                    default:
                        AddInclude(p => p.Name);
                        break;
                }
            }
        }

        // Get Specific Product
        public ProductWithBrandAndTypeSpwcifications(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.productBrand);
            AddInclude(p => p.productType);
        }
    }
}
