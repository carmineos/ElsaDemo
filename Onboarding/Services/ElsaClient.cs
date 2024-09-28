using Elsa.Api.Client.Resources.WorkflowDefinitions.Contracts;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Responses;
using Elsa.Api.Client.Resources.WorkflowDefinitions.Requests;
using Elsa.Api.Client.Resources.WorkflowInstances.Contracts;
using Elsa.Api.Client.Resources.WorkflowInstances.Models;
using System.Text.Json.Nodes;
using Elsa.Api.Client.Extensions;

namespace Onboarding.Services;

/// <summary>
/// A client for the Elsa API.
/// </summary>
public class ElsaClient(HttpClient httpClient, IWorkflowInstancesApi workflowInstancesApi, IExecuteWorkflowApi executeWorkflowApi)
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

        var request = new 
        { 
            Result = result 
        };

        await httpClient.PostAsJsonAsync(url, request, cancellationToken);
    }


    public async Task<string> StartWorklowAsync(string workflowDefinitionId, Dictionary<string, object?>? inputPayload = default, CancellationToken cancellationToken = default)
    {
        var request = new ExecuteWorkflowDefinitionRequest()
        {
            Input = inputPayload,
        };

        var response = await executeWorkflowApi.ExecuteAsync(workflowDefinitionId, request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = content.ConvertTo<ExecuteWorkflowDefinitionResponse>();

        return result!.WorkflowState.Id;
    }

    public async Task<WorkflowInstance?> GetWorkflowInstanceAsync(string workflowInstanceId, CancellationToken cancellationToken = default)
    {
        return await workflowInstancesApi.GetAsync(workflowInstanceId, cancellationToken);
    }

    public async Task UpdateInputsAsync(string workflowInstanceId, Dictionary<string, object> input, CancellationToken cancellationToken = default)
    {

        var instance =  await workflowInstancesApi.GetAsync(workflowInstanceId, cancellationToken);

        if (instance is null)
            return;

        instance.WorkflowState.Input ??= new Dictionary<string, object>();

        foreach (var kvp in input)
        {
            // Try to Add
            if(!instance.WorkflowState.Input.TryAdd(kvp.Key, kvp.Value))
            {
                // Otherwise replace it
                if (instance.WorkflowState.Input.Remove(kvp.Key))
                    instance.WorkflowState.Input.TryAdd(kvp.Key, kvp.Value);
            }
        }

        // TODO: WorkflowInstanceStore not exposed
    }
}

