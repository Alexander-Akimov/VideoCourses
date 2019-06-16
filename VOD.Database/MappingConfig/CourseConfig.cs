using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(80);
            builder.Property(p => p.Description)
                .HasMaxLength(1024);
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(255);
            builder.Property(p => p.MarqueeImageUrl)
                .HasMaxLength(255);

        }
    }
}
