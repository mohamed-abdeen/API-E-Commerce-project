using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, Np => Np.WithOwner());
            builder.Property(o => o.Status)
                .HasConversion(
                oStatus=>oStatus.ToString(),
               oStatus=>(OrderStatus)Enum.Parse(typeof(OrderStatus),oStatus)

                );

            builder.HasMany(o=>o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.SubTotal)
                 .HasColumnType("decimal(18,2)");
        }
    }
}
