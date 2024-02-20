using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopluation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine($"Seeding data ...");

                context.Platforms.AddRange(
                    new Platform(){Name= "Dot net", Publisher= "Microsoft", Cost="Free"},
                    new Platform(){Name= "SQL Server", Publisher= "Microsoft", Cost="Free"},
                    new Platform(){Name= "Angular", Publisher= "Google", Cost="Free"},
                    new Platform(){Name= "React", Publisher= "Facebook", Cost="Free"}
                );

                context.SaveChanges();

            }
            else 
            {
                Console.WriteLine($"We already have data");
            }
        }
    }
}