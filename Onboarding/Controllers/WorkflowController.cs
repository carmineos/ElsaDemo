using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboarding.Data;
using Onboarding.Data.Models.Workflows;
using Onboarding.Services;

namespace Onboarding.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowsController(OnboardingDbContext dbContext, ElsaClient elsaClient) : Controller
{
    private readonly OnboardingDbContext _dbContext = dbContext;
    private readonly ElsaClient _elsaClient = elsaClient;

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateWorkflowRequest request, CancellationToken cancellationToken)
    {
        var workflowRequest = new WorkflowRequest()
        {
            Id = Guid.NewGuid(),
            WorkflowDefinitionId = request.WorkflowDefinitionId,
            CreatedBy = Guid.Empty,
            WorkflowInstanceId = null,
            CreatedAtUtc = DateTimeOffset.Now,
        };

        _dbContext.Add(workflowRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(workflowRequest.Id);
    }

    [HttpGet("{workflowRequestId:guid}")]
    public async Task<IActionResult> GetById(Guid workflowRequestId, CancellationToken cancellationToken)
    {
        var workflowRequest = await _dbContext.WorkflowRequests
            .AsNoTracking()
            //.Include(w => w.TaskRequests)
            .SingleOrDefaultAsync(r => r.Id == workflowRequestId, cancellationToken);

        if (workflowRequest is null)
        {
            return NotFound();
        }

        var workflowInstance = await _elsaClient.GetWorkflowInstanceAsync(workflowRequest.WorkflowInstanceId, cancellationToken);

        return Ok(workflowRequest);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit(StartWorkflowRequest request, CancellationToken cancellationToken)
    {
        var workflowRequest = await _dbContext.WorkflowRequests
            .SingleOrDefaultAsync(r => r.Id == request.WorkflowRequestId, cancellationToken);

        if (workflowRequest is null)
        {
            return NotFound();
        }

        if (workflowRequest.WorkflowInstanceId is not null)
        {
            return BadRequest("Workflow already started");
        }

        var input = new Dictionary<string, object?>()
        {
            { "WorkflowRequestId", workflowRequest.Id },
            { "RequestData", workflowRequest } // Example
        };

        workflowRequest.WorkflowInstanceId = await _elsaClient.StartWorklowAsync(workflowRequest.WorkflowDefinitionId, input, cancellationToken);

        _dbContext.Update(workflowRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(workflowRequest.Id);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateWorkflowRequest request, CancellationToken cancellationToken)
    {
        var workflowRequest = await _dbContext.WorkflowRequests
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == request.WorkflowRequestId, cancellationToken);

        if (workflowRequest is null)
        {
            return NotFound();
        }

        var input = new Dictionary<string, object?>()
        {
            { "WorkflowRequestId", workflowRequest.Id },
            { "RequestData", workflowRequest }, // Example
            { "WORKS", true }
        };

        await _elsaClient.UpdateInputsAsync(workflowRequest.WorkflowInstanceId, input, cancellationToken);

        _dbContext.Update(workflowRequest);

        return Ok();
    }
}

public record CreateWorkflowRequest
{
    public string WorkflowDefinitionId { get; set; } = default!;
}

public record StartWorkflowRequest
{
    public Guid WorkflowRequestId { get; set; }
}

public record UpdateWorkflowRequest
{
    public Guid WorkflowRequestId { get; set; }
}