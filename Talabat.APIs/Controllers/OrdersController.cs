using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService ,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost]  //api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderaddress=  _mapper.Map<AddressDto,Address>(orderDto.ShippingAdderss); 
            var order = await _orderService.CreateOrderAsync(buyerEmail ,orderDto.BasketId,orderDto.DeliveryMethodId, orderaddress);
            if (order == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order)); 
        }
        [HttpGet] //api/orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>>GetOrderForUser()
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderUserAsync(buyeremail);
            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>> (orders));
        }

        [HttpGet("{id}")]  //api/orders/id

        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderIdForUserAsync(id, buyeremail);
            if (order == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order ,OrderToReturnDto>( order));
        }
        [HttpGet("deliverymethod")] //api/orders/deliverymethod

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliverymethod = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliverymethod);
        }


    }
}
