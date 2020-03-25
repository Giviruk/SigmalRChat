using System;
using ChatSignalR.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255);
                entity.Property(e => e.Password).HasColumnName("Password").HasMaxLength(255);
                entity.Property(e => e.Nickname).HasColumnName("Nickname").HasMaxLength(255);
                entity.Property(e => e.RoleId).HasColumnName("RoleId").HasDefaultValue(2);
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);

                entity.HasData(new
                    {
                        Id = 1,
                        Email = "admin@mail.ru",
                        Password = "admin",
                        Nickname = "admin",
                    },
                    new
                    {
                        Id = 2,
                        Email = "test@mail.ru",
                        Password = "test",
                        Nickname = "test",
                    },
                    new
                    {
                        Id = 3,
                        Email = "kek@mail.ru",
                        Password = "kek",
                        Nickname = "kek",
                    });
            });

            // modelBuilder.Entity<Message>(entity =>
            // {
            //     entity.ToTable("Messages");
            //
            //     entity.Property(e => e.Id).HasColumnName("Id");
            //     entity.Property(e => e.UserName).HasColumnName("Username").HasMaxLength(31);
            //     entity.Property(e => e.Text).HasColumnName("Text").HasMaxLength(255);
            //     entity.Property(e => e.Date).HasColumnName("Date");
            //     entity.Property(e => e.UserID).HasColumnName("UserID");
            //     entity.HasOne(e => e.Sender)
            //         .WithMany(m => m.Messages)
            //         .HasForeignKey(k => k.UserID);
            // });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Rooms");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(15);
                entity.Property(e => e.OwnerID).HasColumnName("Owner");
                entity.HasData(
                    new
                    {
                        Id = 1,
                        Name = "Тестовая1",
                        OwnerID = 1,
                    },
                    new
                    {
                        Id = 2,
                        Name = "Тестовая2",
                        OwnerID = 1,
                    },
                    new
                    {
                        Id = 3,
                        Name = "Тестовая3",
                        OwnerID = 1,
                    });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(15);

                entity.HasData(
                    new
                    {
                        Id = 1,
                        Name = "admin",
                    },
                    new
                    {
                        Id = 2,
                        Name = "user",
                    },
                    new
                    {
                        Id = 3,
                        Name = "banned",
                    });
            });

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=chat;Username=postgres;Password=42544321cawa");
            }
        }
    }
}