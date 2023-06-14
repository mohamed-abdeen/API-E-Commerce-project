using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Reposatories;
using Talabat.Core.Services;
using Talabat.Core.Specification;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration
            , IBasketRepo basketRepo,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["stripesettings:Secretkey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) return null;
            var shippingprice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.reposatiory<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingprice = deliveryMethod.Cost;
                basket.ShippingPrice = deliveryMethod.Cost;
            }
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.reposatiory<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * item.Price * 100) + (long)shippingprice * 100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>() {"card"}
                };
                intent=await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecrete = intent.ClientSecret;
            }
            else // update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * item.Price * 100) + (long)shippingprice * 100,

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;

        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentintentId , bool IsSucceeded)
        {
            var spec = new OrderByPaymentIntentIdSPecification(paymentintentId);
            var order = await _unitOfWork.reposatiory<Order>().GetByIdWithSpecAsync(spec);

            if (IsSucceeded)
                order.Status = OrderStatus.PaymentRecived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.reposatiory<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
            
        }
    }
}
