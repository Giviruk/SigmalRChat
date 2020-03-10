using ChatSignalR.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();//создание базы при первом обращении
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
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);

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
            //     entity.HasOne<User>(e => e.Sender)
            //         .WithMany(m => m.Messages)
            //         .HasForeignKey(k => k.UserID);
            // });
            //modelBuilder.Entity<Room>().ToTable("Rooms");
            

                base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=chat;Username=postgres;Password=42544321cawa");
            }
        }
    }
}
