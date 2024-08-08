using Onboarding.Data;
using Onboarding.Entities;
using Onboarding.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NJsonSchema;
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
    public async Task<IActionResult> CreateDraft(CreateWorkflowRequest request, CancellationToken cancellationToken)
    {
        var workflowTemplate = await _dbContext.WorkflowTemplates.SingleOrDefaultAsync(cancellationToken);

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
            WorkflowInstanceId = null
        };

        _dbContext.Add(workflowRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(workflowRequest.Id);
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start(StartWorkflowRequest request, CancellationToken cancellationToken)
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

        workflowRequest.WorkflowInstanceId = await _elsaClient.StartWorklowAsync(workflowRequest.WorkflowTemplate.WorkflowDefinitionId!, workflowRequest.RequestJsonData, cancellationToken);

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