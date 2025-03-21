namespace NewsletterApi.Api.Common.Abstractions;

public interface IEndpoint
{
	void MapEndpoint(IEndpointRouteBuilder app);
}