using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        // Perform the migrate auto operation running the DB context database migrate async method.
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            // Provides auto migration for any pending migrations
            // and/or create database when starting application if not exists
            dbContext.Database.MigrateAsync();

            return app;
        }
    }
}
