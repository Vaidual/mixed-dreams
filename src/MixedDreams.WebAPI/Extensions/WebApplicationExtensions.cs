using Microsoft.EntityFrameworkCore;
using MixedDreams.Infrastructure.Data;

namespace MixedDreams.WebAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    appContext.Database.Migrate();
                }
            }
            return app;
        }
    }
}
