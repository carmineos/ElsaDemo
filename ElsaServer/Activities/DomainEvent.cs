using Elsa.Extensions;
using Elsa.Mediator.Contracts;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Models;

namespace ElsaServer.Activities;

public record DomainEventNotification(ActivityExecutionContext ActivityExecutionContext, string DomainEventId, string DomainEventName, IDictionary<string, object>? DomainEventPayload) : INotification;


[Activity("Elsa", "Primitives", "Requests a given domain event to be published. ", Kind = ActivityKind.Action)]
public class DomainEvent: Activity
{
    /// <summary>
    /// The name of the domain event being published.
    /// </summary>
    [Input(Description = "The name of the domain event being published.")]
    public Input<string> DomainEventName { get; set; } = default!;

    /// <summary>
    /// The payload of the domain event being requested.
    /// </summary>
    [Input(Description = "Any additional parameters to send to the domain event.")]
    public Input<IDictionary<string, object>?> Payload { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var domainEventName = DomainEventName.Get(context);
        var identityGenerator = context.GetRequiredService<IIdentityGenerator>();
        var domainEventId = identityGenerator.GenerateId();

        // Publish the domain event
        var domainEventPayload = Payload.GetOrDefault(context);
        var domainEventNotification = new DomainEventNotification(context, domainEventId, domainEventName, domainEventPayload);
        var dispatcher = context.GetRequiredService<INotificationSender>();

        await dispatcher.SendAsync(domainEventNotification, context.CancellationToken);
        await context.CompleteActivityAsync();
    }
}


public class DomainEventNotificationHandler : INotificationHandler<DomainEventNotification>
{
    public async Task HandleAsync(DomainEventNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.DomainEventName);
    }
}