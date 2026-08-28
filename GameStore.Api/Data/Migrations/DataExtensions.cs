using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        //Db Context has a Scoped Service lifetime because: 
        // 1. it ensures that a new instance/ request of DbContext is created for each HTTP request, which is important for thread safety and to avoid potential issues with shared state.
        // 2. it allows for better resource management, as the DbContext can be disposed
        // 3. Db connextions are a limited and expensive resource, and having a scoped lifetime helps to ensure that connections are not held open longer than necessary.
        // 4. Dbcontext is not thread-safe. Scoped avoids to concurrency issues
        // 5. Makes it easier to manage transactions and ensure data consistency
        // 6. Reusing a DB context instance can lead to increased memory usage
        builder.Services.AddScoped<GameStoreContext>();
        builder.Services.AddSqlite<GameStoreContext>(
    connString,
    optionsAction: options => options.UseSeeding((context,_) =>
    {
        if(!context.Set<Genre>().Any())
        {
            context.Set<Genre>().AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Strategy" }
            );
            context.SaveChanges();
        }
    })
);
    }
} 