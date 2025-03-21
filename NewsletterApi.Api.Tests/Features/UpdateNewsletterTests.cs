using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Endpoints;
using NewsletterApi.Api.Tests.Common.Abstractions;
using NewsletterApi.Api.Tests.Common.Fakers;
using Newtonsoft.Json;

namespace NewsletterApi.Api.Tests.Features;

public sealed class UpdateNewsletterTests : BaseIntegrationTest
{
	public UpdateNewsletterTests(TestWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task UpdateNewsletter_WhenRequestIsInvalid_ReturnsBadRequest()
	{
		var model = new UpdateNewsletterFeature.Request() { Title = null!, Description = null!, Content = null! };
		var json = JsonConvert.SerializeObject(model);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		
		var response = await Client.PutAsync($"/api/Newsletters/{Guid.NewGuid()}", content);
		
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task UpdateNewsletter_WhenNewsletterDoesntExist_ReturnsNotFound()
	{
		var model = new NewsletterFaker().Generate();
		var json = JsonConvert.SerializeObject(model);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		
		var response = await Client.PutAsync($"/api/Newsletters/{Guid.NewGuid()}", content);
		
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task UpdateNewsletter_WhenRequestIsValid_ReturnsNoContent()
	{
		var model = new NewsletterFaker().Generate();
		DbContext.Newsletters.Add(model);
		await DbContext.SaveChangesAsync();
		
		var request = new UpdateNewsletterFeature.Request() { Title = "Test", Description = "Test", Content = "Test" };
		var json = JsonConvert.SerializeObject(request);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var response = await Client.PutAsync($"/api/Newsletters/{model.NewsletterId}", content);
		
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
		var dbModel = await DbContext.Newsletters.SingleAsync(i => i.NewsletterId == model.NewsletterId);
		//Assert.NotNull(dbModel);
		//Assert.Equal(request.Title, dbModel.Title);
		//Assert.Equal(request.Description, dbModel.Description);
		//Assert.Equal(request.Content, dbModel!.Content);
	}
}