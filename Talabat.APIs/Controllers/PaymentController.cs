using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Logging;

namespace Talabat.APIs.Controllers
{

    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private const string _webhook = "whsec_e62cd7bf4b41c28e2b82abcfbee15d18a4214d8aaf55efcb214c6042051fd4eb";
        public PaymentController(IPaymentService paymentService ,ILogger logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("basketid")]  // /api/payments/basketid
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateIntent(string basketid)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketid);
            if (basket == null) return BadRequest(new ApiResponse(400, "A problem with your Basket"));
            return Ok(basket);

        }

        [HttpPost("webhook")]   // https://localhost:5001/api/payments/webhook 

        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhook);

                PaymentIntent intent;
                Order order;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(intent.Id,true);
                        _logger.LogInformation("payment is Succeeded ", order.Id ,intent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(intent.Id,false);
                        _logger.LogInformation("payment is Failed ", order.Id ,intent.Id);

                        break;


                }

            return new EmptyResult();
            }
      
    }


}

