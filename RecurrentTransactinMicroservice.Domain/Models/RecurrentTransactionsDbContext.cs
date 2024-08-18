using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecurrentTransactinMicroservice.Domain.Models;

public partial class RecurrentTransactionsDbContext : DbContext
{
    public RecurrentTransactionsDbContext()
    {
    }

    public RecurrentTransactionsDbContext(DbContextOptions<RecurrentTransactionsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RecurrentTransaction> RecurrentTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=recurrent-transactions-db;Username=username;Password=mysecretpassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecurrentTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recurrenttransaction_pk");

            entity.ToTable("RecurrentTransaction");

            entity.Property(e => e.BranchId).HasColumnType("character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
