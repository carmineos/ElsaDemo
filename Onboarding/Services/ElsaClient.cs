using System.Text.Json.Nodes;

namespace Onboarding.Services;

/// <summary>
/// A client for the Elsa API.
/// </summary>
public class ElsaClient(HttpClient httpClient)
{
    /// <summary>
    /// Reports a task as completed.
    /// </summary>
    /// <param name="taskId">The ID of the task to complete.</param>
    /// <param name="result">The result of the task.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    public async Task ReportTaskCompletedAsync(string taskId, object? result = default, CancellationToken cancellationToken = default)
    {
        var url = new Uri($"tasks/{taskId}/complete", UriKind.Relative);
        var request = new { Result = result };
        await httpClient.PostAsJsonAsync(url, request, cancellationToken);
    }


    public async Task<string> StartWorklowAsync(string workflowDefinitionId, Guid workflowRequestId, string? inputPayload = default, CancellationToken cancellationToken = default)
    {
        var url = new Uri($"workflow-definitions/{workflowDefinitionId}/execute", UriKind.Relative);

        var root = new JsonObject();
        root["input"] = JsonNode.Parse(inputPayload!);

        root["input"].AsObject().Add("WorkflowRequestId", workflowRequestId);

        var response = await httpClient.PostAsJsonAsync(url, root, cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<WorkflowInstance>(cancellationToken);

        return result!.Id;
    }

    public async Task<WorkflowInstance?> GetWorkflowInstanceAsync(string workflowInstanceId, CancellationToken cancellationToken = default)
    {
        var url = new Uri($"workflow-instances/{workflowInstanceId}", UriKind.Relative);

        var response = await httpClient.GetFromJsonAsync<WorkflowInstance>(url, cancellationToken);

        return response;
    }
}


public class WorkflowInstance
{
    public string Id { get; set; } = default!;
    public string DefinitionId { get; set; } = default!;
}
