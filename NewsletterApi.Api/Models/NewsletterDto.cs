using System.ComponentModel.DataAnnotations;

namespace NewsletterApi.Api.Models;

public sealed class NewsletterDto
{
	[Required]
	public Guid NewsletterId { get; init; }

	[Required, MaxLength(150)]
	public string Title { get; init; }
	
	[Required, MaxLength(250)]
	public string Description { get; init; }
	
	[Required]
	public string Content { get; init; }
	
	[Required]
	public DateTimeOffset CreatedOn { get; init; }
}