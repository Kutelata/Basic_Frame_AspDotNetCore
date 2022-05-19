using System.Data;
using Loan2022.Application.Interfaces.Context;
using Loan2022.Application.Interfaces.Shared;
using Loan2022.Domain.Abstracts;
using Loan2022.Domain.Entities;
using Loan2022.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Loan2022.Infrastructure.DbContexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IAuthenticatedUserService _authenticatedUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUser ) : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }

        public IDbConnection Connection => Database.GetDbConnection();
        
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCare> CustomersCare { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<MediaCustomer> MediasCustomer { get; set; }
        public DbSet<Setting> Settings  { get; set; }
        public DbSet<CustomerWalletHistory> CustomerWalletHistorys  { get; set; }
        public DbSet<Interest> Interests  { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests  { get; set; }
        public bool HasChanges => ChangeTracker.HasChanges();
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = _authenticatedUser.UserId != null ?_authenticatedUser.UserId : Users.FirstOrDefaultAsync(x => x.UserName == "SuperAdmin").Result?.Id;
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            // {
            //     entity.SetTableName("l_" + entity.GetTableName().ToLower());
            // }
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }