using NewsletterApi.Api.Database.Entities;
using NewsletterApi.Api.Models;

namespace NewsletterApi.Api.Tests.Common.Extensions;

public static class NewsletterExtensions
{
	public static void AssertEqual(this Newsletter expected, NewsletterDto? actual, bool includeIdentifier = true)
	{
		Assert.NotNull(actual);
		if (includeIdentifier)
		{
			Assert.Equal(expected.NewsletterId, actual.NewsletterId);
		}
		
		Assert.Equal(expected.Title, actual.Title);
		Assert.Equal(expected.Description, actual.Description);
		Assert.Equal(expected.Content, actual.Content);
		Assert.Equal(expected.CreatedOn, actual.CreatedOn);
	}

	public static void AssertEqual(this IEnumerable<Newsletter> expected, IEnumerable<NewsletterDto> actual)
	{
		Assert.NotEmpty(actual);
		Assert.Equal(expected.Count(), actual.Count());
		foreach (var item in expected)
		{
			var actualItem = actual.SingleOrDefault(i => i.NewsletterId == item.NewsletterId);
			item.AssertEqual(actualItem);
		}
	}
}