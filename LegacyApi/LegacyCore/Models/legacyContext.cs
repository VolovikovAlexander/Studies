using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LegacyCore.Models
{
    public partial class legacyContext : DbContext
    {
        public legacyContext()
        {
        }

        public legacyContext(DbContextOptions<legacyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RefAccount> RefAccounts { get; set; }
        public virtual DbSet<RefContract> RefContracts { get; set; }
        public virtual DbSet<RefCustomer> RefCustomers { get; set; }
        public virtual DbSet<RefRecipient> RefRecipients { get; set; }
        public virtual DbSet<RefReportPeriod> RefReportPeriods { get; set; }
        public virtual DbSet<RepCustomerHistory> RepCustomerHistories { get; set; }
        public virtual DbSet<RepCustomerTotal> RepCustomerTotals { get; set; }
        public virtual DbSet<TblTransaction> TblTransactions { get; set; }
        public virtual DbSet<TblTransactionFact> TblTransactionFacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress2014;Database=legacy;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<RefAccount>(entity =>
            {
                entity.Property(e => e.AccountNumber).IsUnicode(false);

                entity.Property(e => e.AccountType).IsUnicode(false);

                entity.Property(e => e.Comments).IsUnicode(false);
            });

            modelBuilder.Entity<RefContract>(entity =>
            {
                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.ContractNumber).IsUnicode(false);
            });

            modelBuilder.Entity<RefCustomer>(entity =>
            {
                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.CustomerType).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<RefRecipient>(entity =>
            {
                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.Descriptions).IsUnicode(false);
            });

            modelBuilder.Entity<RefReportPeriod>(entity =>
            {
                entity.Property(e => e.Comments).IsUnicode(false);
            });

            modelBuilder.Entity<RepCustomerHistory>(entity =>
            {
                entity.Property(e => e.Csv).IsUnicode(false);

                entity.Property(e => e.Pdf).IsUnicode(false);

                entity.Property(e => e.ReportStatus).IsUnicode(false);

                entity.Property(e => e.Xml).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RepCustomerHistories)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_repCustomerAuditHistory_refCustomers");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.RepCustomerHistories)
                    .HasForeignKey(d => d.PeriodId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_repCustomerAuditHistory_refReportPeriods");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.RepCustomerHistories)
                    .HasForeignKey(d => d.RecipientId)
                    .HasConstraintName("FK_repCustomerHistory_refRecipients");
            });

            modelBuilder.Entity<RepCustomerTotal>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RepCustomerTotals)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_repCustomerTotals_refCustomers");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.RepCustomerTotals)
                    .HasForeignKey(d => d.PeriodId)
                    .HasConstraintName("FK_repCustomerTotals_refReportPeriods");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.RepCustomerTotals)
                    .HasForeignKey(d => d.RecipientId)
                    .HasConstraintName("FK_repCustomerTotals_refRecipients");
            });

            modelBuilder.Entity<TblTransaction>(entity =>
            {
                entity.Property(e => e.OperationType).IsUnicode(false);

                entity.Property(e => e.TransactionNumber).IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_tblTransactions_refAccounts");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_tblTransactions_refContracts");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_tblTransactions_refCustomers");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.TblTransactions)
                    .HasForeignKey(d => d.PeriodId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_tblTransactions_refReportPeriods");
            });

            modelBuilder.Entity<TblTransactionFact>(entity =>
            {
                entity.Property(e => e.AccountNumber).IsUnicode(false);

                entity.Property(e => e.Category)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContractNumber).IsUnicode(false);

                entity.Property(e => e.CustomerInn).IsUnicode(false);

                entity.Property(e => e.CustomerNumber).IsUnicode(false);

                entity.Property(e => e.TransactionNumber).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
