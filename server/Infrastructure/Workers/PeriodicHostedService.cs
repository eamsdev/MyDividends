using Application.Features.Dividends;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers;

public class PeriodicHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PeriodicHostedService> _logger;
    
    private static readonly TimeSpan Period = TimeSpan.FromDays(1);

    public bool IsEnabled { get; set; } = true;
    public bool HasErrored { get; private set; }

    public PeriodicHostedService(
        IServiceProvider serviceProvider,
        ILogger<PeriodicHostedService> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        using var periodicTimer = new PeriodicTimer(Period);
        do
        {
            if (token.IsCancellationRequested)
            {
                _logger.LogInformation("Cancellation requested, exiting {JobName} job", 
                    nameof(PeriodicHostedService));
                return;
            }


            if (!IsEnabled)
            {
                _logger.LogInformation("Job is disabled, skipping {JobName} job for this cycle", 
                    nameof(PeriodicHostedService));
                continue;
            }

            
            _logger.LogInformation("Executing {JobName} job", nameof(PeriodicHostedService));
            await ExecuteJob(token);
        } while (!token.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync(token));
    }

    private async Task ExecuteJob(CancellationToken token)
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var securitiesInfoService = scope.ServiceProvider.GetRequiredService<RefreshSecuritiesInfoService>();
            await securitiesInfoService.Refresh(token);
            
            HasErrored = false;
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Failed to execute {NameOfService} job with exception {ExceptionMessage}", 
                nameof(PeriodicHostedService), 
                e.Message);

            HasErrored = true;
        }
    }
}