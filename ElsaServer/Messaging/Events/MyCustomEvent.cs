using MassTransit;

namespace ElsaServer.Messaging.Events;

public record MyCustomEvent(string Text);

public class MyCustomEventHandler : IConsumer<MyCustomEvent>
{
    private readonly ILogger<MyCustomEventHandler> _logger;

    public MyCustomEventHandler(ILogger<MyCustomEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MyCustomEvent> context)
    {
        _logger.LogInformation(context.Message.Text);

        return Task.CompletedTask;
    }
}