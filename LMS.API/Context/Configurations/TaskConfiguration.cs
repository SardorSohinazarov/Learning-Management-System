using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Context.Configurations
{
    public class TaskConfiguration
    {
        public void Configure(EntityTypeBuilder<Models.Task> builder)
        {
            builder.ToTable("tasks");

            builder.HasKey(table => table.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValue("TaskName");
        }
    }
}
