using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LoanManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<LoanSchedule> LoanSchedules => Set<LoanSchedule>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Validation and uniqueness for PersonalNumber
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(c => c.PersonalNumber).IsUnique();
                entity.Property(c => c.FirstName).IsRequired();
                entity.Property(c => c.LastName).IsRequired();
            });

            // Configure relationships
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Customer)
                .WithMany(c => c.Loans)
                .HasForeignKey(l => l.CustomerId);

            modelBuilder.Entity<LoanSchedule>()
                .HasOne(s => s.Loan)
                .WithMany(l => l.Schedules)
                .HasForeignKey(s => s.LoanId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Loan)
                .WithMany(l => l.Payments)
                .HasForeignKey(p => p.LoanId);
        }
    }
}