using System.Net;
using System.Net.Http.Json;
using NewsletterApi.Api.Models;
using NewsletterApi.Api.Tests.Common.Abstractions;
using NewsletterApi.Api.Tests.Common.Extensions;
using NewsletterApi.Api.Tests.Common.Fakers;

namespace NewsletterApi.Api.Tests.Features;

public sealed class GetNewsletterTests : BaseIntegrationTest
{
	public GetNewsletterTests(TestWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task GetNewsletter_WhenNewsletterExists_ReturnsNewsletter()
	{
		var newsletter = new NewsletterFaker().Generate();
		DbContext.Newsletters.Add(newsletter);
		await DbContext.SaveChangesAsync();
		
		var item = await Client.GetFromJsonAsync<NewsletterDto>($"/api/Newsletters/{newsletter.NewsletterId}");
		
		newsletter.AssertEqual(item);
	}

	[Fact]
	public async Task GetNewsletter_WhenNewsletterDoesNotExist_ReturnNotFound()
	{
		var response = await Client.GetAsync($"/api/Newsletters/{Guid.NewGuid()}");
		
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}