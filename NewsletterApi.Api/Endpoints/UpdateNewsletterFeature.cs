using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Common.Abstractions;
using NewsletterApi.Api.Common.Constants;
using NewsletterApi.Api.Common.Extensions;
using NewsletterApi.Api.Database;

namespace NewsletterApi.Api.Endpoints;

public static class UpdateNewsletterFeature
{
	public sealed class Request
	{
		[Required, MaxLength(150)]
		public string Title { get; set; } = string.Empty;
	
		[Required, MaxLength(250)]
		public string Description { get; set; } = string.Empty;
	
		[Required]
		public string Content { get; set; } = string.Empty;
	}
	
	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(i => i.Title).NotEmpty();
			RuleFor(i => i.Description).NotEmpty();
			RuleFor(i => i.Content).NotEmpty();
		}
	}
	
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPut("/Newsletters/{newsletterId:guid}", async (Guid newsletterId, [FromBody] Request model, 
					ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
				{
					var newsletter = await dbContext.Newsletters
						.FirstOrDefaultAsync(i => i.NewsletterId == newsletterId, cancellationToken);
					if (newsletter is null)
					{
						return Results.NotFound();
					}
	
					newsletter.Update(model.Title, model.Description, model.Content);
					await dbContext.SaveChangesAsync(cancellationToken);
					return Results.NoContent();
				})
				.WithName("UpdateNewsletter")
				.WithSummary("Updates a newsletter.")
				.WithDescription("Updates a newsletter with the specified identifier.")
				.WithTags(Tags.Newsletters)
				.WithRequestValidation<Request>()
				.Produces(StatusCodes.Status204NoContent)
				.ProducesProblem(StatusCodes.Status404NotFound)
				.ProducesProblem(StatusCodes.Status500InternalServerError);
		}
	}
}