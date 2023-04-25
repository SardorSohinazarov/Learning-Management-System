using System;
using System.Collections.Generic;
using LMS.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions option)
            : base(option)
        { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<LocalizedStringEntity> LocalizedStrings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LocalizedStringEntity>().HasKey(l => l.Key);
            builder.Entity<LocalizedStringEntity>().HasData(
                new List<LocalizedStringEntity>()
                {
                    new LocalizedStringEntity
                    {
                        Key = "Required",
                        Uz = "{0} kiritilishi shart",
                        Ru = "{0} ... ruscha ...",
                        En = "{0} failed is required"
                    }
                }
            );
        }
    }
}
