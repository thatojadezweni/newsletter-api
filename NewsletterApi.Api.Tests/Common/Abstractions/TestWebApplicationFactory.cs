using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsletterApi.Api.Database;

namespace NewsletterApi.Api.Tests.Common.Abstractions;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			var descriptor = services
				.Single(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
			services.Remove(descriptor);

			services.AddDbContext<ApplicationDbContext>(i =>
			{
				i.UseSqlite("Data Source=ApplicationTest.db", 
					j => j.MigrationsAssembly(typeof(TestWebApplicationFactory).Assembly));
			});

			var serviceProvider = services.BuildServiceProvider();

			var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
		});
	}
}