using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entity.Models
{
    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PetInfo> PetInfo { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("server=localhost;user id=sa;pwd=sa;database=AppDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<PetInfo>(entity =>
            {
                entity.HasKey(e => e.PetId)
                    .HasName("PK__UserInfo__F3BEEBFF28705786");

                entity.Property(e => e.PetId)
                    .HasColumnName("PET_ID")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasMaxLength(255);

                entity.Property(e => e.PetAge).HasColumnName("PET_AGE");

                entity.Property(e => e.PetName)
                    .HasColumnName("PET_NAME")
                    .HasMaxLength(255);

                entity.Property(e => e.PetSex).HasColumnName("PET_SEX");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.PetInfo)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__PetInfo__OWNER_I__5165187F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.Account).HasMaxLength(255);

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PetId).HasMaxLength(255);
            });
        }
    }
}
