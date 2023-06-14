using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Repository.Data.Config;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          //  modelBuilder.ApplyConfiguration(new ProductConfiguratios());
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   
        }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductType> producttypes { get; set; }

        public DbSet<ProductBrand> productBrands { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet <OrderItem> orderItems { get; set; }
        public DbSet<DeliveryMethod> deliveryMethods { get; set; }

    }
}
