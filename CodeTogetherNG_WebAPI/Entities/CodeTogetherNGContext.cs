using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CodeTogetherNG_WebAPI.Entities
{
    public partial class CodeTogetherNGContext : IdentityDbContext<AspNetUsers,AspNetRoles,string
        ,AspNetUserClaims,AspNetUserRoles,AspNetUserLogins,AspNetRoleClaims,AspNetUserTokens>
    {
        public CodeTogetherNGContext()
        {
        }

        public CodeTogetherNGContext(DbContextOptions<CodeTogetherNGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ITRole> Role { get; set; }
        public virtual DbSet<UserITRole> UserRole { get; set; }
        public virtual DbSet<ProjectMember> ProjectMember { get; set; }
        public virtual DbSet<ProjectState> ProjectState { get; set; }
        public virtual DbSet<ProjectTechnology> ProjectTechnology { get; set; }
        public virtual DbSet<Technology> Technology { get; set; }
        public virtual DbSet<UserTechnologyLevel> UserTechnologyLevel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
     
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasDefaultValueSql("(sysdatetimeoffset())");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.Title)
                    .HasName("UC_Project_Title")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.StateId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProject");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Project__StateId__7BE56230");
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MessageDate).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ProjectMember)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectMember_AspNetUsers");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMember)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectMe__Proje__0E04126B");
            });

            modelBuilder.Entity<ProjectState>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProjectTechnology>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.TechnologyId });

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectTechnology)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ProjectTe__Proje__7755B73D");

                entity.HasOne(d => d.Technology)
                    .WithMany(p => p.ProjectTechnology)
                    .HasForeignKey(d => d.TechnologyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectTe__Techn__7849DB76");
            });

            modelBuilder.Entity<Technology>(entity =>
            {
                entity.Property(e => e.TechName).HasMaxLength(20);
            });

            modelBuilder.Entity<UserTechnologyLevel>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.TechnologyId })
                    .HasName("UC_UserTechnologyLevel_UserId_TechnologyId")
                    .IsUnique();

                entity.HasOne(d => d.Technology)
                    .WithMany(p => p.UserTechnologyLevel)
                    .HasForeignKey(d => d.TechnologyId)
                    .HasConstraintName("FK_UserTechnologyLevel_Technology");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTechnologyLevel)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserTechnologyLevel_AspNetUsers");
            });
        }
    }
}
