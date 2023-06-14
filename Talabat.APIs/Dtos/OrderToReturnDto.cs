using System.Collections.Generic;
using System;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 

        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        //public DeliveryMethod DeliveryMethod { get; set; }  // NP one
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } // NP  many
        public decimal SubTotal { get; set; }
        public string PaymentIntenId { get; set; }

        public decimal Total { get; set; }

    }
}
