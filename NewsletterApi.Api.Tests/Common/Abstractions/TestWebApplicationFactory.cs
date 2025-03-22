using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsletterApi.Api.Database;

namespace NewsletterApi.Api.Tests.Common.Abstractions;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
	private SqliteConnection _connection;
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			var descriptor = services
				.Single(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
			services.Remove(descriptor);

			_connection = new SqliteConnection("DataSource=:memory:");
			_connection.Open();
			
			services.AddDbContext<ApplicationDbContext>(i =>
			{
				i.UseSqlite(_connection, 
					j => j.MigrationsAssembly(typeof(TestWebApplicationFactory).Assembly));
			});

			var serviceProvider = services.BuildServiceProvider();

			var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
		});
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		_connection?.Close();
		_connection?.Dispose();
	}
}