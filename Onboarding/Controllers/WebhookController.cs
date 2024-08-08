using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboarding.Data;
using Onboarding.Data.Models.Workflows;
using Onboarding.Models;

namespace Onboarding.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhookController(OnboardingDbContext dbContext) : Controller
{
    [HttpPost("run-task")]
    public async Task<IActionResult> RunTask(WebhookEvent webhookEvent)
    {
        var payload = webhookEvent.Payload;
        var taskPayload = payload.TaskPayload;

        var workflowRequest = await dbContext.WorkflowRequests.SingleOrDefaultAsync(w => w.Id == taskPayload.WorkflowRequestId, CancellationToken.None);

        if (workflowRequest is null)
        {
            throw new Exception();
        }

        var task = new TaskRequest
        {
            // TODO: Add WorkflowRequestId
            WorklowRequestId = workflowRequest!.Id,
            ExternalTaskId = payload.TaskId,
            Name = payload.TaskName,
            Description = taskPayload.Description,
            CreatedAt = DateTimeOffset.Now
        };

        workflowRequest.TaskRequests.Add(task);

        dbContext.Update(workflowRequest);

        await dbContext.SaveChangesAsync(CancellationToken.None);

        return Ok();
    }
}