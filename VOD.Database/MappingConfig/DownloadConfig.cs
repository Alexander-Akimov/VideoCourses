using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class DownloadConfig : IEntityTypeConfiguration<Download>
    {
        public void Configure(EntityTypeBuilder<Download> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(80);
            builder.Property(p => p.Url)
                .HasMaxLength(1024);
        }
    }
}
