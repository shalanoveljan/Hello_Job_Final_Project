using HelloJob.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace HelloJob.Data.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasOne(r => r.Resume)
               .WithMany(r => r.Requests)
               .HasForeignKey(r => r.ResumeId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
         .HasOne(r => r.Vacancy)
         .WithMany()
         .HasForeignKey(r => r.VacancyId)
         .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasAlternateKey(r => new { r.VacancyId, r.AppUserId })
            .HasName("UQ_VacancyId_AppUserId");



        }

    }
}
