using Entities.Common;
using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DBContexts.SQLSERVER
{
    public class HelloJobDbContext:IdentityDbContext<AppUser>
    {

        public HelloJobDbContext(DbContextOptions<HelloJobDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagCourse> TagCourses { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Employee_Special_Education> employee_Special_Educations { get; set; }
        public DbSet<Employee_Special_Experience> employee_Special_Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<About_Vacancy> About_Vacancies { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.Entity<Comment>().HasOne(x => x.AppUser).WithMany(x=>x.Comments).HasForeignKey(x=>x.Id);
            base.OnModelCreating(builder);
        }

    }
}
