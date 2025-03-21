using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Common.Abstractions;
using NewsletterApi.Api.Common.Constants;
using NewsletterApi.Api.Database;
using NewsletterApi.Api.Models;

namespace NewsletterApi.Api.Endpoints;

public static class GetNewslettersFeature
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("/Newsletters", async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
				{
					var newsletters = await dbContext.Newsletters
						.Select(i => new NewsletterDto()
						{
							NewsletterId = i.NewsletterId,
							Title = i.Title,
							Description = i.Description,
							Content = i.Content,
							CreatedOn = i.CreatedOn
						})
						.ToListAsync(cancellationToken);
	
					return Results.Ok(newsletters);
				}).WithName("GetNewsletters")
				.WithSummary("Returns a list of newsletters.")
				.WithDescription("Returns a list of newsletters.")
				.WithTags(Tags.Newsletters)
				.Produces<IEnumerable<NewsletterDto>>()
                .ProducesValidationProblem()
				.ProducesProblem(StatusCodes.Status500InternalServerError); 
		}
	}
}