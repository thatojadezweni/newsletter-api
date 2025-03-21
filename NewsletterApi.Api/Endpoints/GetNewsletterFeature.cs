using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Common.Abstractions;
using NewsletterApi.Api.Common.Constants;
using NewsletterApi.Api.Common.Extensions;
using NewsletterApi.Api.Database;
using NewsletterApi.Api.Models;

namespace NewsletterApi.Api.Endpoints;

public sealed class GetNewsletterFeature
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("{newsletterId:guid}", async (Guid newsletterId, ApplicationDbContext dbContext,
				CancellationToken cancellationToken) =>
			{
				var newsletter = await dbContext.Newsletters
					.AsNoTracking()
					.FirstOrDefaultAsync(i => i.NewsletterId == newsletterId, cancellationToken);
				if (newsletter is null)
				{
					return Results.NotFound();
				}

				var model = newsletter.ToDto();
				return Results.Ok(model);
			})
			.WithName("GetNewsletter")
			.WithSummary("Returns a newsletter.")
			.WithDescription("Returns a newsletter with the provided identifier.")
			.WithTags(Tags.Newsletters)
			.Produces<NewsletterDto>()
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status500InternalServerError);
		}
	}
}