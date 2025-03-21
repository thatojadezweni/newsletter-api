using NewsletterApi.Api.Common.Middlewares;

namespace NewsletterApi.Api.Common.Extensions;

public static class EndpointConventionBuilderExtensions
{
	public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
	{
		return builder.AddEndpointFilter<ValidationFilter<TRequest>>()
			.ProducesValidationProblem();
	}
}