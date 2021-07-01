using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database
{
    public static class Services
    {
        public static ServiceProvider ServiceProvider = new ServiceCollection()
            .AddDbContext<Context>(options =>
            {
                options.UseSqlite($@"Data Source = { DB.dbPath }");
            })
            .BuildServiceProvider();
    }
}
