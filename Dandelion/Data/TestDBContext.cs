using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

#nullable disable

namespace Dandelion.Data
{
    public partial class TestDBContext : DbContext
    {
        public TestDBContext()
        {
        }

        public TestDBContext(DbContextOptions<TestDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SQL5108.site4now.net;Initial Catalog=db_a85cba_info;User Id=db_a85cba_info_admin;Password=12345Vanja");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int");

                entity.HasKey(e => e.Login);
                entity.Property(e => e.Login);

                entity.Property(e => e.Password);

                entity.Property(e => e.Status);

                entity.Property(e => e.Name);
            });
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.Login);

                entity.Property(e => e.Pic);

                entity.Property(e => e.Name);


                entity.Property(e => e.Description);

                entity.Property(e => e.Tag);

                entity.Property(e => e.Float1);
                entity.Property(e => e.Float2);
                entity.Property(e => e.Float3);

                entity.Property(e => e.Line1);
                entity.Property(e => e.Line2);
                entity.Property(e => e.Line3);

                entity.Property(e => e.Text1);
                entity.Property(e => e.Text2);
                entity.Property(e => e.Text3);

                entity.Property(e => e.Date1);
                entity.Property(e => e.Date2);
                entity.Property(e => e.Date3);

                entity.Property(e => e.Checkbox1);
                entity.Property(e => e.Checkbox2);
                entity.Property(e => e.Checkbox3);
            });
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.Name);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Tegs);

                entity.Property(e => e.Float1).HasColumnType("float");
                entity.Property(e => e.Float2).HasColumnType("float");
                entity.Property(e => e.Float3).HasColumnType("float");

                entity.Property(e => e.Line1);
                entity.Property(e => e.Line2);
                entity.Property(e => e.Line3);

                entity.Property(e => e.Text1);
                entity.Property(e => e.Text2);
                entity.Property(e => e.Text3);

                entity.Property(e => e.Date1).HasColumnType("datetime");
                entity.Property(e => e.Date2).HasColumnType("datetime");
                entity.Property(e => e.Date3).HasColumnType("datetime");

                entity.Property(e => e.Checkbox1).HasColumnType("bit");
                entity.Property(e => e.Checkbox2).HasColumnType("bit");
                entity.Property(e => e.Checkbox3).HasColumnType("bit");

                entity.Property(e => e.Pic);

                entity.Property(e => e.CollectionId).HasColumnType("int");
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("int");
                entity.Property(e => e.Name);

                entity.Property(e => e.Text);

                entity.Property(e => e.ItemId).HasColumnType("int");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
