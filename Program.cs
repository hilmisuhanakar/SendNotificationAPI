using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using SendNotificationAPI;
using SendNotificationAPI.EndPoints;
using SendNotificationAPI.Enums;
using SendNotificationAPI.Models.Options;
using SendNotificationAPI.Models.RequestModel;
using SendNotificationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddKeyedTransient<INotificationService<SendMailNotificationRequest>, MailService>(NotificationType.Mail);

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.Headers.RetryAfter = "5";
        await context.HttpContext.Response.WriteAsJsonAsync(new { error = "Too many requests" }, token);
    };

    o.AddConcurrencyLimiter("notification", options =>
    {
        options.PermitLimit = 50;
        options.QueueLimit = 50;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.Configure<SMTPOptions>(builder.Configuration.GetSection("Smtp"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseMiddleware<Middleware>();

app.UseRateLimiter();

app.MapNotification();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow, version = "1.0.0" , appName = "SendNotificationAPI" }));

app.Run();
