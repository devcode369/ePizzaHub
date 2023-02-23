using ePizzaHub.Entities;
using ePizzaHub.Repositories;
using ePizzaHub.Repositories.Implementations;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Services.Configuration
{
    public static class ConfigureRepository
    {
        public static void AddService(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });
            services.AddIdentity<User,Role>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddScoped<DbContext, AppDbContext>();         
            services.AddTransient<IRepository<Item>, Repository<Item>>();
            services.AddTransient<IRepository<Category>, Repository<Category>>();
            services.AddTransient<IRepository<ItemType>, Repository<ItemType>>();
            services.AddTransient<IRepository<CartItem>, Repository<CartItem>>();
            services.AddScoped<ICartRepository, CartRepository>();
        }
    }
}
