using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboarding.Data;
using Onboarding.Models;
using Onboarding.Services;
using Onboarding.Views.Home;

namespace Onboarding.Controllers;

public class HomeController(OnboardingDbContext dbContext, ElsaClient elsaClient, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tasks = await dbContext.TaskRequests
            .AsNoTracking()
            .Include(p => p.TaskType)
            .Include(p => p.WorkflowRequest)
                .ThenInclude(p => p.WorkflowTemplate)
                    .ThenInclude(p => p.WorkflowType)
            .Where(x => !x.IsCompleted)
            .ToListAsync(cancellationToken);

        var model = new IndexViewModel(tasks);
        return View(model);
    }

    public async Task<IActionResult> Approve(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await dbContext.TaskRequests.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);

        if (task == null)
            return NotFound();

        await elsaClient.ReportTaskCompletedAsync(task.ExternalTaskId, result: new HRReviewResult { Approved = true }, cancellationToken: cancellationToken);

        task.IsCompleted = true;
        task.CompletedBy = Guid.Empty;
        task.CompletedAt = DateTimeOffset.Now;

        dbContext.TaskRequests.Update(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Reject(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await dbContext.TaskRequests.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);

        if (task == null)
            return NotFound();

        await elsaClient.ReportTaskCompletedAsync(task.ExternalTaskId, result: new HRReviewResult { Approved = false }, cancellationToken: cancellationToken);

        task.IsCompleted = true;
        task.CompletedBy = Guid.Empty;
        task.CompletedAt = DateTimeOffset.Now;

        dbContext.TaskRequests.Update(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class HRReviewResult
{
    public bool Approved { get; set; }
}