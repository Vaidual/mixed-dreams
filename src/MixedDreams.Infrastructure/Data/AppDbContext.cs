﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Domain.Common;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MixedDreams.Application.Data
{
    internal class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly Guid? _tenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantGetter) : base(options) 
        {
            _tenantId = tenantGetter.TenantId;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            builder.Entity<Product>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
            builder.Entity<Ingredient>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
            builder.Entity<Order>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
            builder.Entity<BusinessLocation>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
            builder.Entity<ProductHistory>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
            builder.Entity<ProductIngredient>().HasQueryFilter(a => _tenantId == null || a.TenantId == _tenantId);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<BusinessLocation> BusinessLocations { get; set; }
        public DbSet<ProductIngredient> ProductIngredient { get; set; }
        public DbSet<ProductHistory> ProductHistory { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<BackblazeFile> BackblazeFiles { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<ProductPreparation> ProductPreparations { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_tenantId is not null)
            {
                foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.TenantId = (Guid)_tenantId!;
                            break;
                        case EntityState.Modified:
                            entry.Entity.TenantId = (Guid)_tenantId!;
                            break;
                    }
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
