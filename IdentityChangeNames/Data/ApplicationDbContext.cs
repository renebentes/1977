using System;
using IdentityChangeNames.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityChangeNames.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User");

            builder.Entity<ApplicationRole>().ToTable("Role");

            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");

            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");

            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(iu => iu.Email).HasColumnName("EmailAddress");
            });
        }
    }
}