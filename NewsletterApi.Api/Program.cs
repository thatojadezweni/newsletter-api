using Microsoft.EntityFrameworkCore;
using NewsletterApi.Api.Common.Extensions;
using NewsletterApi.Api.Database;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
builder.Services.AddDbContext<ApplicationDbContext>(i =>
{
	i.UseSqlite(builder.Configuration.GetConnectionString("Database"))
		.EnableDetailedErrors();
});

var app = builder.Build();

var routeGroupBuilder = app.MapGroup("/api");
app.MapEndpoints(routeGroupBuilder);

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options =>
	{
		options
			.WithTheme(ScalarTheme.Kepler)
			.WithDarkModeToggle(true)
			.WithClientButton(true);
	});
}

app.UseHttpsRedirection();

app.Run();

public partial class Program { }