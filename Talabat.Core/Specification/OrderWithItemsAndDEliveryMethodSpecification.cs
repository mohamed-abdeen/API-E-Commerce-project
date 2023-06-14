using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification
{
    public class OrderWithItemsAndDEliveryMethodSpecification : BaseSpecification<Order>
    {

        public OrderWithItemsAndDEliveryMethodSpecification(string buyerEmail)
        : base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);

        }

        public OrderWithItemsAndDEliveryMethodSpecification(int ordderId ,string buyerEmail)
  : base(O => O.BuyerEmail == buyerEmail && O.Id==ordderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);


        }
    }
}
