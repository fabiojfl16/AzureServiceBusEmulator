namespace ConsumerApi;

public class EventListenerHostedService : IHostedService, IAsyncDisposable
{
    private readonly EventListener _listener;
    private readonly ILogger<EventListenerHostedService> _logger;

    public EventListenerHostedService(IConfiguration config, ILogger<EventListenerHostedService> logger)
    {
        _logger = logger;
        var connectionString = config.GetConnectionString("messaging");
        var topicName = "topic";
        var subscriptionName = "mysubscription";

        _listener = new EventListener(_logger, connectionString!, topicName, subscriptionName);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting EventListenerHostedService...");
        await _listener.StartAsync(cancellationToken);
        _logger.LogInformation("EventListenerHostedService started.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping EventListenerHostedService...");
        await _listener.StopAsync(cancellationToken);
        _logger.LogInformation("EventListenerHostedService stopped.");
    }

    public ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing EventListenerHostedService...");
        return _listener.DisposeAsync();
    }
}