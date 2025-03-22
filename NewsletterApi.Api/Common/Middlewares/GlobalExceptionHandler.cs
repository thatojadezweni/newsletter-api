using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NewsletterApi.Api.Common.Middlewares;

public sealed class GlobalExceptionHandler(IWebHostEnvironment webHostEnvironment, 
	ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
		CancellationToken cancellationToken)
	{
		logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
		
		var problemDetails = new ProblemDetails
		{
			Instance = exception.GetType().Name,
			Status = StatusCodes.Status500InternalServerError,
			Title = "Internal server error",
			Detail = exception.Message
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		if (webHostEnvironment.IsProduction())
		{
			problemDetails.Detail = "An error occurred.";
		}

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
		return true;
	}
}