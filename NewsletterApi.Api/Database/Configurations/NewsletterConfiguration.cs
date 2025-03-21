using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsletterApi.Api.Database.Entities;

namespace NewsletterApi.Api.Database.Configurations;

public sealed class NewsletterConfiguration : IEntityTypeConfiguration<Newsletter>
{
	public void Configure(EntityTypeBuilder<Newsletter> builder)
	{
		builder.ToTable("Newsletter");

		builder.HasKey(i => i.NewsletterId);

		builder.Property(i => i.Title)
			.HasMaxLength(150);

		builder.Property(i => i.Description)
			.HasMaxLength(250);

		builder.Property(i => i.Content)
			.HasColumnType("text");
	}
}