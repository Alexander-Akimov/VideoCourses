using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class InstructorConfig : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(80);
            builder.Property(p => p.Description)
                .HasMaxLength(1024);
            builder.Property(p => p.Thumbnail)
                 .HasMaxLength(1024);
        }
    }
}
