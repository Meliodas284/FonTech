using FonTech.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FonTech.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.Property(x => x.Login).IsRequired().HasMaxLength(100);
		builder.Property(x => x.Password).IsRequired();

		builder.HasMany<Report>(x => x.Reports)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId)
			.HasPrincipalKey(x => x.Id);

		builder.HasMany<Role>(x => x.Roles)
			.WithMany(x => x.Users)
			.UsingEntity<UserRole>(
				x => x.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
				x => x.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
			);

		builder.HasData(new List<User>
		{
			new User()
			{
				Id = 1,
				Login = "Meliodas",
				Password = new string('-', 20),
				CreatedAt = DateTime.UtcNow
			}
		});
	}
}
