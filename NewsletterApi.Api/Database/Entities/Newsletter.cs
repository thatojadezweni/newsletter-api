namespace NewsletterApi.Api.Database.Entities;

public sealed class Newsletter
{
	public Guid NewsletterId { get; private set; }

	public string Title { get; private set; } = null!;
	
	public string Description { get; private set; }
	
	public string Content { get; private set; }
	
	public DateTimeOffset CreatedOn { get; private set; }

	public static Newsletter Create(string title, string description, string content, 
		TimeProvider timeProvider)
	{
		return new()
		{
			NewsletterId = Guid.NewGuid(),
			Title =	title,
			Description = description,
			Content = content,
			CreatedOn = timeProvider.GetUtcNow()
		};
	}

	public void Update(string title, string description, string content)
	{
		Title = title;
		Description = description;
		Content = content;
	}
}