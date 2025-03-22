using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Endpoints;
using NewsletterApi.Api.Models;
using NewsletterApi.Api.Tests.Common.Abstractions;
using NewsletterApi.Api.Tests.Common.Fakers;
using Newtonsoft.Json;

namespace NewsletterApi.Api.Tests.Features;

public sealed class CreateNewsletterTests : BaseIntegrationTest
{
	public CreateNewsletterTests(TestWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task CreateNewsletter_WhenRequestIsInvalid_ReturnsBadRequest()
	{
		var model = new CreateNewsletterFeature.Request() { Title = null!, Description = null!, Content = null! };
		var json = JsonConvert.SerializeObject(model);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		
		var response = await Client.PostAsync("/api/Newsletters", content);
		
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task CreateNewsletter_WhenRequestIsValid_CreatesNewsletter()
	{
		var model = new NewsletterFaker().Generate();
		var json = JsonConvert.SerializeObject(model);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		
		var response = await Client.PostAsync("/api/Newsletters", content);
		
		var newsletter = JsonConvert.DeserializeObject<NewsletterDto>(await response.Content.ReadAsStringAsync());
		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		Assert.NotNull(newsletter);
		Assert.Equal(newsletter.Title, model.Title);
		Assert.Equal(newsletter.Description, model.Description);
		Assert.Equal(newsletter.Content, model.Content);
		Assert.Equal(newsletter.CreatedOn.Date, model.CreatedOn.Date);
		
		var dbModel = await DbContext.Newsletters.SingleAsync(i => i.NewsletterId == newsletter.NewsletterId);
		Assert.Equal(dbModel.NewsletterId, newsletter.NewsletterId);
	}
}