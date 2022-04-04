using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Models;

namespace SubscriptionManager.Data
{
    public class SubscriptionManagerContext : DbContext
    {
        public SubscriptionManagerContext (DbContextOptions<SubscriptionManagerContext> options)
            : base(options)
        {
        }

        public DbSet<SubscriptionManager.Models.Subscription> Subscription { get; set; }

        public DbSet<SubscriptionManager.Models.User> User { get; set; }

        public DbSet<SubscriptionManager.Models.Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }



    }
}
