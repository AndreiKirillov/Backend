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
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Subscription> Subscription { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Subscription>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();

            // Добавляем категории в бд
            modelBuilder.Entity<Category>().HasData(new Category
            { Id = 1, Title = "Music", Description = "This category is used for music services" });
            modelBuilder.Entity<Category>().HasData(new Category
            { Id = 2, Title = "Movies", Description = "This category is used for movies services" });
            modelBuilder.Entity<Category>().HasData(new Category
            { Id = 3, Title = "Job", Description = "This category is used for job services" });
            modelBuilder.Entity<Category>().HasData(new Category
            { Id = 4, Title = "Hobby", Description = "This category is used for different hobby services" });
            modelBuilder.Entity<Category>().HasData(new Category
            { Id = 5, Title = "Sport", Description = "This category is used for sport activities" });

        }



    }
}
