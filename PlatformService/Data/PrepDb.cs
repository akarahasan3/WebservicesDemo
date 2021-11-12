﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatformService.Model;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                    throw;
                }
            }
            
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                
                context.Platforms.AddRange(
                    new Platform() {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"});
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}