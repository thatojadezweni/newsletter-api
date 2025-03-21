using System.ComponentModel.DataAnnotations;

namespace NewsletterApi.Api.Models;

public abstract class UpsertNewsletterDto
{
	[Required, MaxLength(150)]
	public string Title { get; set; } = string.Empty;
	
	[Required, MaxLength(250)]
	public string Description { get; set; } = string.Empty;
	
	[Required]
	public string Content { get; set; } = string.Empty;
}