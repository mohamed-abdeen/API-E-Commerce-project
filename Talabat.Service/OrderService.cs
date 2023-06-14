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

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenaricReposatiory<Product> _productrepo;
        //private readonly IGenaricReposatiory<DeliveryMethod> _deliverymethodrepo;
        //private readonly IGenaricReposatiory<Order> _orderrepo;

        public OrderService(
            IBasketRepo basketRepo,
            //IGenaricReposatiory<Product> productrepo,
            //IGenaricReposatiory<DeliveryMethod> deliverymethodrepo,
            //IGenaricReposatiory<Order> orderrepo
           IUnitOfWork unitOfWork
            ,IPaymentService paymentService
            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productrepo = productrepo;
            //_deliverymethodrepo = deliverymethodrepo;
            //_orderrepo = orderrepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            // get basket from basket repo
            var basket = await _basketRepo.GetBasketAsync(basketId);


            //get selected items at basket from product repo
            var orderitems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.reposatiory<Product>().GetByIdAsync(item.Id);

                var productItemorder = new ProductItemOrder(product.Id ,product.Name ,product.PictureUrl);
                var orderItem = new OrderItem(productItemorder,product.Price ,item.Quantity);
                orderitems.Add(orderItem);


            }

            //calculate subtotal
            var subTotal = orderitems.Sum(item => item.Price = item.Quantity);

            //get delivery method from deliverymethod repo
            var deliverymethod = await _unitOfWork.reposatiory<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // Create Order
            var spec = new OrderByPaymentIntentIdSPecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.reposatiory<Order>().GetByIdWithSpecAsync(spec);

            if(existingOrder !=null)
            {
                _unitOfWork.reposatiory<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            var order = new Order(buyerEmail, ShippingAddress, deliverymethod, orderitems, subTotal ,basket.PaymentIntentId);
            await _unitOfWork.reposatiory<Order>().CreateAsync(order);
            //save to database
        var result=  await _unitOfWork.Complete();
            if (result <= 0) return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliverymethod= await _unitOfWork.reposatiory<DeliveryMethod>().GetAllAsync();
            return deliverymethod;
        }

        public async Task<Order> GetOrderIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDEliveryMethodSpecification(orderId ,buyerEmail);
            var order = await _unitOfWork.reposatiory<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDEliveryMethodSpecification(buyerEmail);
            var orders = await _unitOfWork.reposatiory<Order>().GetAllwithSpecificationAsync(spec);
            return orders;
        }
    }
}
