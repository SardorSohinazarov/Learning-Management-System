using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Context.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(t => t.Id);
            builder.Property(user => user.FirstName).IsRequired();

            builder.HasMany(user => user.Courses)
                .WithOne(userCourse => userCourse.User)
                .HasForeignKey(userCourse => userCourse.UserId);
        }
    }
}
