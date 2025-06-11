﻿using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler
    (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        // use c# pattern matching to handle specific exceptions
        (string Detail, string Title, int statusCode) details = exception switch
        {
            InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            UnauthorizedAccessException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status401Unauthorized
            ),
            ArgumentException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            _ => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.statusCode,
            Type = exception.GetType().Name
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationException", validationException.Errors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
