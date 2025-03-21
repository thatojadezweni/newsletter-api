using System.Net;
using NewsletterApi.Api.Tests.Common.Abstractions;
using NewsletterApi.Api.Tests.Common.Fakers;

namespace NewsletterApi.Api.Tests.Features;

public sealed class DeleteNewsletterTests : BaseIntegrationTest
{
	public DeleteNewsletterTests(TestWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task DeleteNewsletter_WhenNewsletterDoesntExist_ReturnsNotFound()
	{
		var response = await Client.DeleteAsync($"api/Newsletters/{Guid.NewGuid()}");
		
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task DeleteNewsletter_WhenNewsletterExists_ReturnsNoContent()
	{
		var newsletter = new NewsletterFaker().Generate();
		DbContext.Newsletters.Add(newsletter);
		await DbContext.SaveChangesAsync();
		
		var response = await Client.DeleteAsync($"/api/Newsletters/{newsletter.NewsletterId}");
		
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}
}