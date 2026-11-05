using EFCoreAssignment03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreAssignment03.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

           
            builder.Property(s => s.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(s => s.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.DateOfBirth)
                   .IsRequired();

            builder.HasMany(s => s.Enrollments)
                   .WithOne(e => e.Student)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
