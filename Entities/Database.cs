using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Entities
{
    public class Database : IdentityDbContext<User>
    {
        public DbSet<BankCardEntity> BankCards { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        public Database(DbContextOptions<Database> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankCardEntity>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.BankCard)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BankCardId);
        }


    }
}
