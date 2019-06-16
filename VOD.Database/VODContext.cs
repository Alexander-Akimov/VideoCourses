using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VOD.Database.MappingConfig;
using VOD.Domain.Entities;

namespace VOD.Database
{
    public class VODContext : IdentityDbContext<VODUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Video> Videos { get; set; }

        public VODContext(DbContextOptions<VODContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CourseConfig());
            builder.ApplyConfiguration(new DownloadConfig());
            builder.ApplyConfiguration(new InstructorConfig());
            builder.ApplyConfiguration(new ModuleConfig());
            builder.ApplyConfiguration(new UserCourseConfig());
            builder.ApplyConfiguration(new VideoConfig());

            //restrict cascading deletes
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
