using Microsoft.Extensions.DependencyInjection;
using NewsletterApi.Api.Database;

namespace NewsletterApi.Api.Tests.Common.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>, IDisposable
{
	protected readonly HttpClient Client;
	protected readonly ApplicationDbContext DbContext;

	protected BaseIntegrationTest(TestWebApplicationFactory factory)
	{
		var scope = factory.Services.CreateScope();
		DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		Client = factory.CreateClient();
	}
	
	public void Dispose()
	{
		DbContext?.Dispose();
	}
}