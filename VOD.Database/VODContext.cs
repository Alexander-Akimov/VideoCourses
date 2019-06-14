using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VOD.Domain.Entities;

namespace VOD.Database
{
    public class VODContext : IdentityDbContext<VODUser>
    {

        public VODContext(DbContextOptions<VODContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
    }
}
