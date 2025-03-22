using NewsletterApi.Api.Database.Entities;
using NewsletterApi.Api.Models;

namespace NewsletterApi.Api.Common.Extensions;

public static class NewsletterMapper
{
	public static NewsletterDto ToDto(this Newsletter @this)
	{
		return new()
		{
			NewsletterId = @this.NewsletterId,
			Title = @this.Title,
			Description = @this.Description,
			Content = @this.Content,
			CreatedOn = @this.CreatedOn
		};
	}
}