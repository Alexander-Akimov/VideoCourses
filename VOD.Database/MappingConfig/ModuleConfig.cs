using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class ModuleConfig : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(80);
        }
    }
}
