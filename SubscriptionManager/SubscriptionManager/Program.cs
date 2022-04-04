using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubscriptionManager.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using SubscriptionManager.Models;

namespace SubscriptionManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();
            
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SubscriptionManagerContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            using (SubscriptionManagerContext db = new SubscriptionManagerContext(options))
            {
                Category music = new Category { Id = 1, Title = "Music", Description = "This category is used for music services" };
                Category movie = new Category { Id = 2, Title = "Movies", Description = "This category is used for movies services" };
                Category job = new Category { Id = 3, Title = "Job", Description = "This category is used for job services" };
                Category hobby = new Category { Id = 4, Title = "Hobby", Description = "This category is used for different hobby services" };
                Category sport = new Category { Id = 5, Title = "Sport", Description = "This category is used for sport activities" };

                db.Category.Add(music);
                db.Category.Add(movie);
                db.Category.Add(job);
                db.Category.Add(hobby);
                db.Category.Add(sport);


                User usr1 = new User { Id = 1, FirstName = "Andrey", LastName = "Kirillov", Subs = null };
                User usr2 = new User { Id = 2, FirstName = "John", LastName = "Johnson", Subs = null };
                User usr3 = new User { Id = 3, FirstName = "Svetlana", LastName = "Petrova", Subs = null };

                db.User.Add(usr1);
                db.User.Add(usr2);
                db.User.Add(usr3);
                optionsBuilder.LogTo(System.Console.WriteLine);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
