﻿using System;
using System.Collections.Generic;
using Lesson10Areas.Models;
using Lesson10Areas.Models.Contstraints;
using Microsoft.EntityFrameworkCore;

namespace Lesson10Areas.Data;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext()
    {
    }

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);

            entity.HasMany(d => d.Tags).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductTag",
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    j =>
                    {
                        j.HasKey("ProductId", "TagId");
                        j.ToTable("ProductTag");
                        j.HasIndex(new[] { "TagId" }, "IX_ProductTag_TagId");
                    });
        });
       
        //modelBuilder.Entity<Product>().HasData(new Product());
        //modelBuilder.Entity<Product>().Property("Name").IsRequired();
        //modelBuilder.ApplyConfiguration(new ProductConfigration());
    }
}
