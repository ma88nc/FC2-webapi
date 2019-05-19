using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlashCardsAPI.Models.DB
{
    public partial class FlashCards2Context : DbContext
    {
        DbContextOptions<FlashCards2Context> _context;
        public FlashCards2Context(DbContextOptions<FlashCards2Context> context)
        {
            _context = context;
        }

        public virtual DbSet<Domains> Domains { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<QuestionTags> QuestionTags { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<TestAttempts> TestAttempts { get; set; }
        public virtual DbSet<Titles> Titles { get; set; }
        public virtual DbSet<UserAnswers> UserAnswers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connString = _context.FindExtension<Microsoft.EntityFrameworkCore.Infrastructure.Internal.SqlServerOptionsExtension>().ConnectionString;
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domains>(entity =>
            {
                entity.HasKey(e => e.DomainId);

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.Property(e => e.QuestionId)
                    .HasColumnName("QuestionID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Answer)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DomainId)
                    .HasColumnName("DomainID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_Domains");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TitleId)
                    .HasConstraintName("FK_Questions_Titles");
            });

            modelBuilder.Entity<QuestionTags>(entity =>
            {
                entity.HasKey(e => e.QuestionTagId);

                entity.Property(e => e.QuestionTagId).HasColumnName("QuestionTagID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.QuestionTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionTags_Tags");
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.DomainId)
                    .HasColumnName("DomainID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ParentTagId).HasColumnName("ParentTagID");

                entity.Property(e => e.TagDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tags_Domains");
            });

            modelBuilder.Entity<TestAttempts>(entity =>
            {
                entity.HasKey(e => e.TestAttemptId);

                entity.Property(e => e.TestAttemptId)
                    .HasColumnName("TestAttemptID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.TestDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.TestAttempts)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestAttempts_Domains");
            });

            modelBuilder.Entity<Titles>(entity =>
            {
                entity.HasKey(e => e.TitleId);

                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DomainId)
                    .HasColumnName("DomainID")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.Titles)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Titles_Domains");
            });

            modelBuilder.Entity<UserAnswers>(entity =>
            {
                entity.HasKey(e => e.UserAnswerId);

                entity.Property(e => e.UserAnswerId)
                    .HasColumnName("UserAnswerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.TestAttemptId).HasColumnName("TestAttemptID");

                entity.Property(e => e.UserAnswer)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAnswers_Questions");

                entity.HasOne(d => d.TestAttempt)
                    .WithMany(p => p.UserAnswers)
                    .HasForeignKey(d => d.TestAttemptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAnswers_TestAttempts");
            });
        }
    }
}
