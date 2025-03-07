using api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace api.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = 500,
            Title = "Server error",
            Detail = exception.Message
        };

        switch (exception)
        {
            case NotFoundException:
                problemDetails.Title = "Resource not found";
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;

            case BadRequestException:
                problemDetails.Title = "Bad request";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;

            case UnauthorizedException:
                problemDetails.Title = "Unauthorized";
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                break;

            case ForbiddenException:
                problemDetails.Title = "Forbidden";
                problemDetails.Status = StatusCodes.Status403Forbidden;
                break;

            // Database errors
            case PostgresException pgEx:
                problemDetails.Status = StatusCodes.Status400BadRequest;

                switch (pgEx.SqlState)
                {
                    case "23505": // Unique constraint violation
                        problemDetails.Title = "Unique constraint violation";
                        problemDetails.Detail =
                            "The record could not be saved because a duplicate value already exists.";
                        break;

                    case "23503": // Foreign key violation
                        problemDetails.Title = "Foreign key constraint violation";
                        problemDetails.Detail = "A related entity does not exist.";
                        break;

                    case "23502": // Not-null violation
                        problemDetails.Title = "Not-null constraint violation";
                        problemDetails.Detail = "A required field was not provided.";
                        break;

                    case "22001": // String too long
                        problemDetails.Title = "String too long";
                        problemDetails.Detail = "The input exceeds the allowed length.";
                        break;

                    case "22P02": // Invalid UUID format
                        problemDetails.Title = "Invalid UUID format";
                        problemDetails.Detail = "The provided UUID is not valid.";
                        break;

                    default:
                        problemDetails.Title = "PostgreSQL error";
                        problemDetails.Detail = "An unknown database error occurred.";
                        break;
                }

                break;

            default:
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                // Don't expose internal error details in production
                if (!httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    problemDetails.Detail = "An internal server error occurred.";
                }

                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check if the inner exception is a PostgreSQL exception with code 23505 (unique violation)
        return ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505";
    }
}