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
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasMany(v => v.Requests) 
                .WithOne(r => r.Vacancy) 
                .HasForeignKey(r => r.VacancyId) 
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
