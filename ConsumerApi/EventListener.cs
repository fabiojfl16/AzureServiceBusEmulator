using Azure.Messaging.ServiceBus;

namespace ConsumerApi;

public class EventListener : IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<EventListenerHostedService> _logger;

    public EventListener(ILogger<EventListenerHostedService> logger, string connectionString, string topicName, string subscriptionName)
    {
        _logger = logger;
        _client = new ServiceBusClient(connectionString);

        _processor = _client.CreateProcessor(topicName, subscriptionName);
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await _processor.StopProcessingAsync(cancellationToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        _logger.LogInformation($"Received message: {body}");

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError($"Error: {args.Exception.Message}");
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }
}
