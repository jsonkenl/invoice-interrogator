using InvoiceInterrogator.Core;
using Microsoft.EntityFrameworkCore;

namespace InvoiceInterrogator.Infrastructure
{
    public class InvoiceInterrogatorDbContext : DbContext 
    {
        public InvoiceInterrogatorDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<InvoiceAccount> InvoiceAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Invoices)
                .WithOne(i => i.Vendor);

            modelBuilder.Entity<InvoiceAccount>()
                .HasKey(ia => new { ia.InvoiceId, ia.AccountId });

            modelBuilder.Entity<InvoiceAccount>()
                .HasOne(ia => ia.Invoice)
                .WithMany(i => i.InvoiceAccounts)
                .HasForeignKey(ia => ia.InvoiceId);

            modelBuilder.Entity<InvoiceAccount>()
                .HasOne(ia => ia.Account)
                .WithMany(a => a.InvoiceAccounts)
                .HasForeignKey(ia => ia.AccountId);
        }
    }
}
