using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NJsonSchema;
using Onboarding.Data;
using Onboarding.Data.Models.Workflows;
using Onboarding.Services;
using System.Text.Json;

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
        var workflowTemplate = await _dbContext.WorkflowTemplates.SingleOrDefaultAsync(w => w.Id == request.WorkflowTemplateId, cancellationToken);

        if (workflowTemplate is null)
        {
            return NotFound();
        }

        // TODO: Build RequestJsonSchema -> actual schema

        var schema = await JsonSchema.FromJsonAsync(workflowTemplate.RequestJsonSchema, cancellationToken);

        var validationResult = schema.Validate(request.RequestData.GetRawText());

        if (validationResult.Count is not 0)
        {
            return BadRequest(validationResult);
        }

        var workflowRequest = new WorkflowRequest()
        {
            Id = Guid.NewGuid(),
            WorkflowTemplateId = workflowTemplate.Id,
            CreatorId = Guid.Empty,
            RequestorId = Guid.Empty,
            RequestJsonData = request.RequestData.GetRawText(),
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
            .Include(r => r.WorkflowTemplate)
                .ThenInclude(r => r.WorkflowType)
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
            .Include(r => r.WorkflowTemplate)
            .SingleOrDefaultAsync(r => r.Id == request.WorkflowRequestId, cancellationToken);

        if (workflowRequest is null)
        {
            return NotFound();
        }

        if(workflowRequest.WorkflowInstanceId is not null)
        {
            return BadRequest("Workflow already started");
        }

        workflowRequest.WorkflowInstanceId = await _elsaClient.StartWorklowAsync(workflowRequest.WorkflowTemplate.WorkflowDefinitionId!, workflowRequest.Id, workflowRequest.RequestJsonData, cancellationToken);

        _dbContext.Update(workflowRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(workflowRequest.Id);
    }
}

public record CreateWorkflowRequest
{
    public int CompanyId { get; set; }
    public int WorkflowTemplateId { get; set; }
    public JsonElement RequestData { get; set; }
}

public record StartWorkflowRequest
{
    public int CompanyId { get; set; }
    public Guid WorkflowRequestId { get; set; }
}