using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VOD.Domain.Entities;

namespace VOD.Database.MappingConfig
{
    internal class UserCourseConfig : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            builder.HasKey(uc => new { uc.UserId, uc.CourseId });

            //builder.HasOne(pt => pt.Prescriber)
            //    .WithMany(p => p.PrescriberAttributeValues)
            //    .HasForeignKey(pt => pt.PrescriberId);

            //builder.HasOne(pt => pt.AttributeValue)
            //    .WithMany(p => p.PrescriberAttributeValues)
            //    .HasForeignKey(pt => pt.AttributeValueId);
        }
    }
}
