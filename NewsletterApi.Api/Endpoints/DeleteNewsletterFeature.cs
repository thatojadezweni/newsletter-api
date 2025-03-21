using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Common.Abstractions;
using NewsletterApi.Api.Common.Constants;
using NewsletterApi.Api.Database;

namespace NewsletterApi.Api.Endpoints;

public static class DeleteNewsletterFeature
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapDelete("{newsletterId:guid}", async (Guid newsletterId, ApplicationDbContext dbContext, 
				CancellationToken cancellationToken) =>
			{
				if (!await dbContext.Newsletters.AnyAsync(i => i.NewsletterId == newsletterId, cancellationToken))
				{
					return Results.NotFound();
				}
	
				await dbContext.Newsletters
					.Where(i => i.NewsletterId == newsletterId)
					.ExecuteDeleteAsync(cancellationToken);
				return Results.NoContent();
			})
			.WithName("DeleteNewsletter")
			.WithSummary("Deletes a newsletter.")
			.WithDescription("Deletes a newsletter with the specified identifier.")
			.WithTags(Tags.Newsletters)
			.Produces(StatusCodes.Status204NoContent)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status500InternalServerError);
		}
	}
}