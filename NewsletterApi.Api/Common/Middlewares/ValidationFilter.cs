using FluentValidation;

namespace NewsletterApi.Api.Common.Middlewares;

public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var request = context.Arguments.OfType<TRequest>().First();

		var result = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
		if (!result.IsValid)
		{
			return Results.ValidationProblem(result.ToDictionary());
		}
		
		return await next(context);
	}
}