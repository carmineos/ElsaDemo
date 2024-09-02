using MassTransit;
using Onboarding.Data.Models.Workflows;

namespace ElsaServer.Messaging.Events;

// Duplicated from ElsaServer (TODO: Share contracts)
public record AbsenceRequestApprovedEvent(string WorkflowRequestId);

public class AbsenceRequestApprovedEventHandler : IConsumer<AbsenceRequestApprovedEvent>
{
    private readonly ILogger<AbsenceRequestApprovedEventHandler> _logger;

    public AbsenceRequestApprovedEventHandler(ILogger<AbsenceRequestApprovedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AbsenceRequestApprovedEvent> context)
    {
        _logger.LogInformation("Absence Request {WorkflowRequestId} Approved", context.Message.WorkflowRequestId);

        return Task.CompletedTask;
    }
}