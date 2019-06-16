using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class VideoConfig : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(80);
            builder.Property(p => p.Description)
                .HasMaxLength(1024);
            builder.Property(p => p.Thumbnail)
                .HasMaxLength(1024);
            builder.Property(p => p.Url)
                .HasMaxLength(1024);
        }
    }
}
