using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleWebApp.Data.SysLog;
using SimpleWebApp.Domain.Entities;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Data;

public class SimpleDBContext : DbContext
{
    public SimpleDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Name).IsRequired();
            entity.HasIndex(c => c.Name).IsUnique();
            entity.HasMany(c => c.Employees).WithMany(e => e.Companies);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Title).IsRequired()
                .HasConversion<EnumToStringConverter<EmployeeTitle>>();
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasMany(e => e.Companies).WithMany(c => c.Employees);
        });

        modelBuilder.Entity<SystemLog>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Event).IsRequired()
                .HasConversion<EnumToStringConverter<SystemLogType>>();
            entity.Property(s => s.Comment).IsRequired();
        });
    }
}