using ATM.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATM.Infrastructure
{
    public class AtmDbContext(DbContextOptions<AtmDbContext> options) : DbContext(options)
    {
        public DbSet<Card>Cards{ get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardHolder)
                .WithMany(ch => ch.Cards)
                .HasForeignKey(c => c.CardHolderId);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Card)
                .HasForeignKey<Account>(a => a.CardId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Card)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CardId);
        }
    }
}
