using Elsa.Mediator.Contracts;
using Elsa.Workflows.Runtime.Notifications;

namespace ElsaServer;

public class RunTaskRequestHandler : INotificationHandler<RunTaskRequest>
{
    public Task HandleAsync(RunTaskRequest notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
