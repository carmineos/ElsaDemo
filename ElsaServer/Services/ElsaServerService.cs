using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Parameters;

namespace ElsaServer.Services;

public class ElsaServerService(
    IWorkflowInstanceManager workflowInstanceManager,
    IWorkflowRuntime workflowRuntime,
    ITaskReporter taskReporter)
{
    public async Task<string> StartWorklowAsync(string workflowDefinitionId, Dictionary<string, object>? inputPayload = default, CancellationToken cancellationToken = default)
    {
        var request = new StartWorkflowRuntimeParams()
        {
            Input = inputPayload,
        };

        var response = await workflowRuntime.StartWorkflowAsync(workflowDefinitionId, request);

        return response!.WorkflowInstanceId;
    }

    public async Task UpdateInputsAsync(string workflowInstanceId, Dictionary<string, object> input, CancellationToken cancellationToken = default)
    {
        var instance = await workflowInstanceManager.FindByIdAsync(workflowInstanceId, cancellationToken);

        if (instance is null)
            return;

        instance.WorkflowState.Input ??= new Dictionary<string, object>();

        foreach (var kvp in input)
        {
            // Try to Add
            if (!instance.WorkflowState.Input.TryAdd(kvp.Key, kvp.Value))
            {
                // Otherwise replace it
                if (instance.WorkflowState.Input.Remove(kvp.Key))
                    instance.WorkflowState.Input.TryAdd(kvp.Key, kvp.Value);
            }
        }

        await workflowInstanceManager.SaveAsync(instance, cancellationToken);
    }

    public async Task ReportTaskCompletedAsync(string taskId, object? result = default, CancellationToken cancellationToken = default)
    {
        await taskReporter.ReportCompletionAsync(taskId, result, cancellationToken);
    }
}
