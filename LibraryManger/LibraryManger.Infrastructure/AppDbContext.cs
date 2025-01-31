using LibraryManger.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace LibraryManger.Infrastructure
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Active = true,
                        Name = "User 1",
                        Created = new DateTime(2025,1,1),
                        Updated = new DateTime(2025,1,1),
                        Id = 1,
                        Email = "user1@email.com"
                    },
                    new User
                    {
                        Active = true,
                        Name = "User 2",
                        Created = new DateTime(2025,1,1),
                        Updated = new DateTime(2025,1,1),
                        Id = 2,
                        Email = "user2@email.com"
                    }
                );

            modelBuilder.Entity<Book>()
                .HasData(
                    new Book
                    {
                        Active = true,
                        Author = "Author 1",
                        Created = new DateTime(2025,1,1),
                        Updated = new DateTime(2025,1,1),
                        Id = 1,
                        IsBorrowed = false,
                        Title = "Book 1"
                    },
                    new Book
                    {
                        Active = true,
                        Author = "Author 2",
                        Created = new DateTime(2025,1,1),
                        Updated = new DateTime(2025,1,1),
                        Id = 2,
                        IsBorrowed = false,
                        Title = "Book 2"
                    },
                    new Book
                    {
                        Active = true,
                        Author = "Author 3",
                        Created = new DateTime(2025,1,1),
                        Updated = new DateTime(2025,1,1),
                        Id = 3,
                        IsBorrowed = false,
                        Title = "Book 3"
                    });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
    }
}