using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Phishing_Platform_Midterm.Entities;

namespace Phishing_Platform_Midterm.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Phishingtemplate> Phishingtemplates { get; set; }

    public virtual DbSet<Sentemail> Sentemails { get; set; }

    public virtual DbSet<Targetemail> Targetemails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userinteraction> Userinteractions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PhishingPlatform;Username=iremnuryilmaz;Password=iny111101;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Phishingtemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phishingtemplate_pkey");

            entity.ToTable("phishingtemplate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Templatemail)
                .HasMaxLength(255)
                .HasColumnName("templatemail");
        });

        modelBuilder.Entity<Sentemail>(entity =>
        {
            entity.HasKey(e => e.Emailid).HasName("sentemail_pkey");

            entity.ToTable("sentemail");

            entity.Property(e => e.Emailid).HasColumnName("emailid");
            entity.Property(e => e.Clickedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("clickedat");
            entity.Property(e => e.Isclicked).HasColumnName("isclicked");
            entity.Property(e => e.Sentat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sentat");
            entity.Property(e => e.Targetid).HasColumnName("targetid");
            entity.Property(e => e.Templateid).HasColumnName("templateid");

            entity.HasOne(d => d.Target).WithMany(p => p.Sentemails)
                .HasForeignKey(d => d.Targetid)
                .HasConstraintName("sentemail_targetid_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.Sentemails)
                .HasForeignKey(d => d.Templateid)
                .HasConstraintName("sentemail_templateid_fkey");
        });

        modelBuilder.Entity<Targetemail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("targetemail_pkey");

            entity.ToTable("targetemail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Targetemail1)
                .HasMaxLength(255)
                .HasColumnName("targetemail");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RegistrationDate).HasColumnName("registrationDate");
            entity.Property(e => e.Sourcepage)
                .HasMaxLength(255)
                .HasColumnName("sourcepage");
        });

        modelBuilder.Entity<Userinteraction>(entity =>
        {
            entity.HasKey(e => e.Interactionid).HasName("userinteraction_pkey");

            entity.ToTable("userinteraction");

            entity.Property(e => e.Interactionid).HasColumnName("interactionid");
            entity.Property(e => e.Emailid).HasColumnName("emailid");
            entity.Property(e => e.Interactiondetail).HasColumnName("interactiondetail");
            entity.Property(e => e.Interactiontime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("interactiontime");
            entity.Property(e => e.Interactiontype)
                .HasMaxLength(255)
                .HasColumnName("interactiontype");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Email).WithMany(p => p.Userinteractions)
                .HasForeignKey(d => d.Emailid)
                .HasConstraintName("userinteraction_emailid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Userinteractions)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("userinteraction_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
