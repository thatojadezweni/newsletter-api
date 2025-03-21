using System.Net.Http.Json;
using NewsletterApi.Api.Models;
using NewsletterApi.Api.Tests.Common.Abstractions;
using NewsletterApi.Api.Tests.Common.Extensions;
using NewsletterApi.Api.Tests.Common.Fakers;

namespace NewsletterApi.Api.Tests.Features;

public sealed class GetNewslettersTests : BaseIntegrationTest
{
	public GetNewslettersTests(TestWebApplicationFactory factory) 
		: base(factory)
	{
		
	}

	[Fact]
	public async Task GetNewsletters_WhenNewslettersDoesnttExist_ShouldReturnNoNewsletters()
	{
		var newsletters = await Client.GetFromJsonAsync<IEnumerable<NewsletterDto>>("/api/Newsletters");
		
		Assert.Empty(newsletters!);
	}

	[Fact]
	public async Task GetNewsletters_WhenNewslettersExist_ShouldReturnNewsletters()
	{
		var newsletters = new NewsletterFaker().Generate(5);
		DbContext.Newsletters.AddRange(newsletters);
		await DbContext.SaveChangesAsync();
		
		var items = await Client.GetFromJsonAsync<IEnumerable<NewsletterDto>>("/api/Newsletters");
		
		newsletters.AssertEqual(items!);
	}
}