using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Persistence;

public class BasketAndWishlistDbMigrator : IHostedService
{
    private readonly IServiceProvider _services;

    public BasketAndWishlistDbMigrator(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<BasketAndWishlistContext>();
                ctx.Database.Migrate();
            }
        }
        catch (Exception ex)
        {}

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
