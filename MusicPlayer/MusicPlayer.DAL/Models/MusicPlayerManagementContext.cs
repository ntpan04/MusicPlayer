using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MusicPlayer.DAL.Models;

public partial class MusicPlayerManagementContext : DbContext
{
    public MusicPlayerManagementContext()
    {
    }

    public MusicPlayerManagementContext(DbContextOptions<MusicPlayerManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Include> Includes { get; set; }

    public virtual DbSet<PlayList> PlayLists { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5861DF95B68");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Include>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Include");

            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.SongId).HasColumnName("SongID");

            entity.HasOne(d => d.PlayList).WithMany()
                .HasForeignKey(d => d.PlayListId)
                .HasConstraintName("FK__Include__PlayLis__5070F446");

            entity.HasOne(d => d.Song).WithMany()
                .HasForeignKey(d => d.SongId)
                .HasConstraintName("FK__Include__SongID__4F7CD00D");
        });

        modelBuilder.Entity<PlayList>(entity =>
        {
            entity.HasKey(e => e.PlayListId).HasName("PK__PlayList__38709FBBFC9AE391");

            entity.ToTable("PlayList");

            entity.Property(e => e.PlayListId)
                .ValueGeneratedNever()
                .HasColumnName("PlayListID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.PlayListName)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.PlayLists)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__PlayList__Accoun__4BAC3F29");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("PK__Song__12E3D6F7D3359D35");

            entity.ToTable("Song");

            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.Artist)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FilePath).HasColumnType("text");
            entity.Property(e => e.SongName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
