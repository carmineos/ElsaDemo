using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using ElsaServer;
using ElsaServer.Messaging.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddElsa(elsa =>
{
    elsa.UseFluentStorageProvider();

    // Configure Management layer to use EF Core.
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

    // Configure Runtime layer to use EF Core.
    elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

    // Default Identity features for authentication/authorization.
    elsa.UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
        identity.UseAdminUserProvider();
    });

    // Configure ASP.NET authentication/authorization.
    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

    // Expose Elsa API endpoints.
    elsa.UseWorkflowsApi();

    // Setup a SignalR hub for real-time updates from the server.
    elsa.UseRealTimeWorkflows();

    // Enable C# workflow expressions
    elsa.UseCSharp();

    // Enable HTTP activities.
    elsa.UseHttp();

    // Use timer activities.
    elsa.UseScheduling();

    // Register custom activities from the application, if any.
    elsa.AddActivitiesFrom<Program>();

    // Register custom workflows from the application, if any.
    elsa.AddWorkflowsFrom<Program>();

    elsa.UseJavaScript();

    elsa.UseEmail(email =>
    {
        email.ConfigureOptions = options =>
        {
            options.Host = "localhost";
            options.Port = 2525;
        };
    });

    elsa.UseWebhooks(webhooks =>
    {
        webhooks.WebhookOptions = options =>
        {
            builder.Configuration.GetSection("Webhooks").Bind(options);
        };

        webhooks.HttpClientBuilder = (httpClientBuilder) =>
        {
            httpClientBuilder.AddHttpMessageHandler<TestDelegatingHandler>();
        };
    });

    // Example using
    // docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
    elsa.UseMassTransit(massTransit =>
    {
        massTransit.UseRabbitMq(
            //"amqp://guest:guest@localhost:5672/elsa", Don't use vhost for the example
            "amqp://guest:guest@localhost:5672",
            rabbitMqFeature => rabbitMqFeature.ConfigureServiceBus = bus =>
            {
                bus.PrefetchCount = 4;
                bus.Durable = true;
                bus.AutoDelete = false;
                bus.ConcurrentMessageLimit = 32;
                // etc. 
            }
        );

        massTransit.AddMessageType<AbsenceRequestApprovedEvent>();
        // Consumer moved to Onboarding project
        //massTransit.AddConsumer<MyCustomEventHandler>(null, false);
    });
});

// Add MediatR integration
builder.Services.AddHandlersFrom<Program>();

builder.Services.AddTransient<TestDelegatingHandler>();

// Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id"))); // Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.

// Add Health Checks.
builder.Services.AddHealthChecks();

// Build the web application.
var app = builder.Build();

// Configure web application's middleware pipeline.
app.UseCors();
app.UseRouting(); // Required for SignalR.
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 

app.Run();