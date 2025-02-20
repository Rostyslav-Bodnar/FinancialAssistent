using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Entities
{
    public class Database : IdentityDbContext<User>
    {
        public DbSet<BankCardEntity> BankCards { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<Icons> Icons { get; set; }

        public DbSet<Widgets> Widgets { get; set; }

        public DbSet<UserInfo> UsersInfo { get; set; }

        public Database(DbContextOptions<Database> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserInfo>()
                .HasOne(u => u.User)
                .WithOne(ui => ui.UserInfo)
                .HasForeignKey<UserInfo>(ui => ui.UserId);

            modelBuilder.Entity<BankCardEntity>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.BankCard)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BankCardId);

            modelBuilder.Entity<Widgets>()
                .HasOne(w => w.UserInfo)
                .WithMany(ui => ui.Widgets)
                .HasForeignKey(w => w.UserInfoId);

            modelBuilder.Entity<Widgets>()
                .HasOne(w => w.Icon)
                .WithMany()
                .HasForeignKey(w => w.IconID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BankCardEntity>()
        .Property(b => b.Balance)
        .HasPrecision(18, 2);

            modelBuilder.Entity<TransactionEntity>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransactionEntity>()
                .Property(t => t.OperationAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransactionEntity>()
                .Property(t => t.BalanceAfterTransaction)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransactionEntity>()
                .Property(t => t.CashbackAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.DailyCostsLimits)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.WeeklyCostsLimits)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.MonthlyBudget)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Widgets>()
                .Property(w => w.Budget)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Widgets>()
                .Property(w => w.Expenses)
                .HasPrecision(18, 2);
        }


    }
}
