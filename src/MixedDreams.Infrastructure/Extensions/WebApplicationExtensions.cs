using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MixedDreams.Application.Data;

namespace MixedDreams.Application.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IHost MigrateDatabase(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                appContext.Database.Migrate();
            }
            return app;
        }
    }
}
