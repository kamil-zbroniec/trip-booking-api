namespace TripBooking.Api.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unexpected error requesting {Method} {Url}",
                context.Request.Method,
                context.Request.GetEncodedPathAndQuery());
            
            await WriteProblemDetailsResponse(context, exception, context.RequestAborted);
        }
    }

    private static Task WriteProblemDetailsResponse(
        HttpContext context,
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Title = "Unexpected error",
            Type = "Unexpected"
        };
        
        problemDetails.Errors.Add("error", [
            exception.GetType().Name,
            exception.Message
        ]);
        
        problemDetails.Instance = $"{context.Request.Method} {context.Request.GetEncodedPathAndQuery()}";
        problemDetails.Detail ??= problemDetails.Title;
        problemDetails.Status ??= StatusCodes.Status500InternalServerError;

        context.Response.StatusCode = problemDetails.Status.Value;

        return context
            .Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}