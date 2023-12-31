﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId,int deliveryMethodId ,Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderUserAsync(string buyerEmail);
        Task<Order> GetOrderIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    
    }
}
