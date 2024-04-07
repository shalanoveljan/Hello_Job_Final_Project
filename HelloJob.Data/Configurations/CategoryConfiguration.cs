using HelloJob.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children).
                 OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(c => c.ParentId);
           

            builder.HasMany(x => x.Blogs)
               .WithOne(x => x.Category)
               .HasForeignKey(x => x.CategoryId);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
