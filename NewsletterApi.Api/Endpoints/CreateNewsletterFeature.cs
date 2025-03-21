using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NewsletterApi.Api.Common.Abstractions;
using NewsletterApi.Api.Common.Constants;
using NewsletterApi.Api.Common.Extensions;
using NewsletterApi.Api.Database;
using NewsletterApi.Api.Database.Entities;

namespace NewsletterApi.Api.Endpoints;

public static class CreateNewsletterFeature
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
	
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("", async ([FromBody] Request model, TimeProvider timeProvider, 
				ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
			{
				var newsletter = Newsletter.Create(model.Title, model.Description, model.Content, timeProvider);
	
				dbContext.Newsletters.Add(newsletter);
				await dbContext.SaveChangesAsync(cancellationToken);
	
				return Results.Ok(newsletter.ToDto());
			})
			.WithName("CreateNewsletter")
			.WithSummary("Creates a newsletter.")
			.WithDescription("Creates a newsletter.")
			.WithTags(Tags.Newsletters)
			.Produces(StatusCodes.Status201Created)
			.ProducesValidationProblem()
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status500InternalServerError);
		}
	}
}