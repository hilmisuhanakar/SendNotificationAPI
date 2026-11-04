using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendNotificationAPI.Enums;
using SendNotificationAPI.Models.RequestModel;
using SendNotificationAPI.Services;

namespace SendNotificationAPI.EndPoints
{
    public static class Notification
    {
        public static IEndpointRouteBuilder MapNotification(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/notification");

            group.MapGet("/", HealthCheck)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Notification")
            .RequireRateLimiting("notification");

            group.MapPost("/mail", SendMailNotification)
            .DisableAntiforgery()
            .Accepts<SendMailNotificationRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Notification")
            .RequireRateLimiting("notification");

            return app;
        }

        private static async Task<IResult> HealthCheck([FromKeyedServices(NotificationType.Mail)] INotificationService<SendMailNotificationRequest> notificationService)
        {
            await notificationService.SendNotificationAsync(new SendMailNotificationRequest
            {
                Subject = "Health Check",
                Body = "Health Check",
                Recipients = new[] { "test@test.com" },
            });
            return Results.Ok();
        }

        private static async Task<IResult> SendMailNotification(SendMailNotificationRequest request, [FromKeyedServices(NotificationType.Mail)] INotificationService<SendMailNotificationRequest> notificationService)
        {
            await notificationService.SendNotificationAsync(request);
            return Results.Ok();
        }
    }
}