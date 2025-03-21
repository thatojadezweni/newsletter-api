using Bogus;
using NewsletterApi.Api.Database.Entities;

namespace NewsletterApi.Api.Tests.Common.Fakers;

public sealed class NewsletterFaker : Faker<Newsletter>
{
	public NewsletterFaker()
	{
		RuleFor(i => i.NewsletterId, i => i.Random.Guid());
		RuleFor(i => i.Title, i => i.Rant.Random.Word());
		RuleFor(i => i.Description, i => i.Rant.Random.Words());
		RuleFor(i => i.Content, i => i.Rant.Review());
		RuleFor(i => i.CreatedOn, i => TimeProvider.System.GetUtcNow());
	}
}