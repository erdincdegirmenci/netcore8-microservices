using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MicroStack.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStack.Infrastructure.Data
{
    public class WebAppContextSeed
    {
        public static async Task SeedAsync(WebAppContext webAppContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retreyForAvailability = retry.Value;
            try
            {
                webAppContext.Database.Migrate();
                if (!webAppContext.AppUsers.Any())
                {
                    await webAppContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retreyForAvailability < 50)
                {
                    retreyForAvailability++;
                    var log = loggerFactory.CreateLogger<WebAppContextSeed>();
                    log.LogError(ex.Message);
                    Thread.Sleep(2000);
                    await SeedAsync(webAppContext, loggerFactory, retreyForAvailability);
                }
            }
        }
        private static IEnumerable<AppUser> GetPreconfiguredOrders()
        {
            return new List<AppUser>()
            {
                new AppUser
                {
                    FirstName ="User1",
                    LastName = "User LastName1",
                    IsSeller = true,
                    IsBuyer = false
                }
            };
        }
    }

}