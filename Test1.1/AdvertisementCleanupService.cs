using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;

public class AdvertisementCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<AdvertisementCleanupService> _logger;

    public AdvertisementCleanupService(IServiceProvider services, ILogger<AdvertisementCleanupService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Advertisement Cleanup Service running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                    var expiredAds = await context.JobAdvertisments
                        .Where(a => a.IsActive && a.ExpiryDate <= DateTime.Now)
                        .ToListAsync();

                    foreach (var ad in expiredAds)
                    {
                        ad.IsActive = false;
                        _logger.LogInformation($"Deactivated expired ad ID: {ad.Id}");
                    }

                    if (expiredAds.Any())
                    {
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred checking for expired advertisements");
            }

            // Run once per day
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}