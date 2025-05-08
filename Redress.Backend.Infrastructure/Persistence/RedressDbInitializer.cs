using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redress.Backend.Infrastructure.Persistence
{
    public class RedressDbInitializer
    {
        public static void Initialize(RedressDbContext context) 
        {
            context.Database.EnsureCreated();
        }
    }
}
