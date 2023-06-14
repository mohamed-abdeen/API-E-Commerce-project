using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Reposatories;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenaricReposatiory<Product> _productrepo;
        private readonly IMapper _mapper;
        private readonly IGenaricReposatiory<ProductBrand> _brandrepo;
        private readonly IGenaricReposatiory<ProductType> _typesrepo;

        public ProductController(
            IGenaricReposatiory<Product> productrepo,
            IMapper mapper,
            IGenaricReposatiory<ProductBrand> brandrepo,
            IGenaricReposatiory<ProductType> typesrepo


            )
        {
            _productrepo = productrepo;
            _mapper = mapper;
            _brandrepo = brandrepo;
            _typesrepo = typesrepo;
        }
        [Authorize]
        [HttpGet] //api/product
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProduct([FromQuery]ProductSpecificationPrams productparams)
        {
            var spec = new ProductWithBrandAndTypeSpwcifications(productparams);



            var products = await _productrepo.GetAllwithSpecificationAsync(spec);

            
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countspec = new ProductWithFilterForCountSpec(productparams);
            var count = await _productrepo.GetCountAsync(countspec);
            return Ok(new Pagination<ProductToReturnDto>(productparams.PageIndex ,productparams.pageSize,count ,Data));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetproductById(int id)
        {

            var spec = new ProductWithBrandAndTypeSpwcifications(id);

            var products = await _productrepo.GetByIdWithSpecAsync(spec);

            if (products == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(products));


        }

        [HttpGet("brands")] // api/product/brands

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandrepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")] // api/products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> Gettypes()
        {
            var types = await _typesrepo.GetAllAsync();
            return Ok(types);
        }
    }
}
