using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Reposatories;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepo basketRepo ,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        [HttpGet] //api/basket/1
        public async Task<ActionResult<CustomerBasket>> GetBasketbyId(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));  
        }
        [HttpPost] //api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedbasket=_mapper.Map<CustomerBasketDto,CustomerBasket>(basket);

            var UpdatedOrCreated = await _basketRepo.UpdateBasketAsync(mappedbasket);
            return Ok(UpdatedOrCreated);
        }

        [HttpDelete] //api/basket
        public async Task DeletaBasket(string id)
        {
            await _basketRepo.DeleteBasket(id);
        }

    }
}
